using Autofac;
using Mapster;
using InventoryManagement.Presentation.Models;
using InventoryManagement.Service.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
using System.Linq.Dynamic.Core;
using System.Text;
using Humanizer;
using InventoryManagement.Presentation.Others;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Presentation.Controllers
{
	[Authorize]
    public class CategoryController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<CategoryController> _logger;
        private readonly LinkGenerator _linkGenerator;

        public CategoryController(ILifetimeScope scope,
            ILogger<CategoryController> logger,
            LinkGenerator linkGenerator)
        {
            _scope = scope;
            _logger = logger;
            _linkGenerator = linkGenerator;
        }

        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Create()
        {
            var model = new CreateCategoryModel();
            model.Resolve(_scope);
            return View(model);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateCategoryModel model)
		{
			if (!ModelState.IsValid)
			{
                _logger.LogInformation("Model state is not valid");
				return View(model);
			}
            model.CreatedAtUtc = DateTime.UtcNow;
			var categoryDto = await model.BuildAdapter().AdaptToTypeAsync<CreateCategoryDto>();
			model.Resolve(_scope);
			await model.CreateCategoryAsync(categoryDto);

			return RedirectToAction("Create");
		}

        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> Get()
		{
			var url = _linkGenerator.GetUriByAction(HttpContext, controller: "Category", action: "Get");
			if (url is null)
			{
				return View();
			}
			var viewModel = new CategoriesModel { EndpointUrl = url };
			return View(viewModel);
		}

        [Authorize(Policy = "UserPolicy")]
        [HttpPost]
		public async Task<IActionResult> Get([FromBody] TabulatorQueryDto dto)
		{
			var model = new CategoriesModel();
			model.Resolve(_scope);
			var allCategoryDetails = await model.GetCategoriesAsync(dto.Page, dto.Size);

			var dataSet = allCategoryDetails;
			var queryable = dataSet.data.AsQueryable();
			var count = 0;

			IQueryable<CategoriesDto>? filteredData = null;

			if (dto.Filters.Count > 0)
			{
				var expression = ExpressionMaker(
					new List<string> { "id","name", "description", "createdAtUtc", "id" },
					new List<string>(),
					dto.Filters
				);
				filteredData = queryable.Where(expression);
				count = allCategoryDetails.total;
			}

			if (dto.Sorters.Count > 0)
			{
				var elem = dto.Sorters[0];
				var expression = $"x => {elem.Field.Pascalize()} {elem.Dir.ToUpper()}";

				if (filteredData is null)
				{
					filteredData = queryable.OrderBy(expression);
					count = allCategoryDetails.total;
				}
				else
				{
					filteredData = filteredData.OrderBy(expression);
				}
			}

			if (filteredData is null)
			{
				filteredData = queryable;
				count = allCategoryDetails.total;
			}

			var totalPages = (int)Math.Ceiling(count / (decimal)dto.Size);
			return Ok(new { data = filteredData, last_row = count, last_page = totalPages });
		}

		private static string ExpressionMaker(IList<string> allowedColumns, IList<string> enumColumns, IList<TabulatorFilterDto> filters)
		{
			var expression = new StringBuilder();
			expression.Append("x => ");

			for (var i = 0; i < filters.Count; i++)
			{
				var filter = filters[i];
				var fieldInPascalCase = filter.Field.Pascalize();

				if (allowedColumns.Contains(filter.Field) is false)
				{
					continue;
				}

				if (enumColumns.Contains(filter.Field))
				{
					expression.Append($"{fieldInPascalCase} = {fieldInPascalCase}.{filter.Value.Pascalize()}");
				}
				else if (filter.Type == FilterHelper.Like)
				{
					if (fieldInPascalCase == "Id")
					{
						expression.Append(
							$"""{fieldInPascalCase}.ToString().Contains("{filter.Value}", StringComparison.InvariantCultureIgnoreCase)"""
						);
					}
					else
					{
						expression.Append(
							$"""{fieldInPascalCase}.Contains("{filter.Value}", StringComparison.InvariantCultureIgnoreCase)"""
						);
					}
				}
				else if (fieldInPascalCase == "CreatedAtUtc")
				{
					var dateTimeValue = DateTime.ParseExact(filter.Value, "dd/MM/yyyy", null);
					expression.Append($"{fieldInPascalCase} {filter.Type} DateTime({dateTimeValue.Year}, {dateTimeValue.Month}, {dateTimeValue.Day})");
				}
				else
				{
					expression.Append($"{fieldInPascalCase} {filter.Type} {filter.Value}");
				}

				if (i + 1 != filters.Count)
				{
					expression.Append(" && ");
				}
			}

			return expression.ToString();
		}

        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Edit(Guid id)
		{
			if (id == Guid.Empty)
			{
				id = (Guid)TempData["Id"];
			}
			else
			{
				TempData["Id"] = id;
			}
			var model = new EditCategoryModel();
			model.Resolve(_scope);
			model = await model.GetCategoryByIdAsync(id);
			TempData["CreatedAtUtc"] = model.CreatedAtUtc;

			return View(model);
		}

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditCategoryModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			if (TempData.ContainsKey("CreatedAtUtc"))
			{
				model.CreatedAtUtc = (DateTime)TempData["CreatedAtUtc"];
			}

			model.UpdatedAtUtc = DateTime.UtcNow;
			var response = await model.BuildAdapter().AdaptToTypeAsync<EditCategoryDto>();
			model.Resolve(_scope);
			await model.EditCategoryAsync(response);
			return RedirectToAction("Get");
		}

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				var model = new DeleteCategoryModel();
				model.Resolve(_scope);
				await model.DeleteCategoryByIdAsync(id);
				return Json(new { success = true, message = "Category deleted successfully" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Error deleting category: " + ex.Message });
			}
			return RedirectToAction("Get");
		}
	}
}