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
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Presentation.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<SupplierController> _logger;
        private readonly LinkGenerator _linkGenerator;
		private readonly string _imageDirectory = "wwwroot/uploads";
		public SupplierController(ILifetimeScope scope,
            ILogger<SupplierController> logger,
            LinkGenerator linkGenerator)
        {
            _scope = scope;
            _logger = logger;
            _linkGenerator = linkGenerator;
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create()
        {
            var model = new CreateSupplierModel();
            model.Resolve(_scope);            
			return View(model);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateSupplierModel model)
		{
			if (!ModelState.IsValid)
			{
                _logger.LogInformation("Model state is not valid");
				return View(model);
			}

			if (!Directory.Exists(_imageDirectory))
			{
				Directory.CreateDirectory(_imageDirectory);
			}

			if (model.ImageFile != null && model.ImageFile.Length > 0)
			{
				var uniqueFileName = $"{Guid.NewGuid()}.jpg";
				var filePath = Path.Combine(_imageDirectory, uniqueFileName);

				using (var imageStream = model.ImageFile.OpenReadStream())
				using (var image = Image.FromStream(imageStream))
				{
					var resizedImage = ResizeImage(image, 250, 333); // Resize to 3:4 aspect ratio
					resizedImage.Save(filePath, ImageFormat.Jpeg);
				}
				model.PhotoUrl = $"/uploads/{uniqueFileName}";
			}
            model.CreatedAtUtc = DateTime.UtcNow;
            var supplierDto = await model.BuildAdapter().AdaptToTypeAsync<CreateSupplierDto>();
			model.Resolve(_scope);
			await model.CreateSupplierAsync(supplierDto);
			return RedirectToAction("Create");
		}

		private Image ResizeImage(Image img, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(img.HorizontalResolution, img.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
				graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

				using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
				{
					wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
					graphics.DrawImage(img, destRect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}
			return destImage;
		}

        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> Get()
		{
			var url = _linkGenerator.GetUriByAction(HttpContext, controller: "Supplier", action: "Get");
			if (url is null)
			{
				return View();
			}
			var viewModel = new SuppliersModel { EndpointUrl = url };
			return View(viewModel);
		}

        [Authorize(Policy = "UserPolicy")]
        [HttpPost]
		public async Task<IActionResult> Get([FromBody] TabulatorQueryDto dto)
		{
			var model = new SuppliersModel();
			model.Resolve(_scope);
			var allSupplierDetails = await model.GetSuppliersAsync(dto.Page, dto.Size);

			var dataSet = allSupplierDetails;
			var queryable = dataSet.data.AsQueryable();
			var count = 0;

			IQueryable<SuppliersDto>? filteredData = null;

			if (dto.Filters.Count > 0)
			{
				var expression = ExpressionMaker(
					new List<string> { "id","photoUrl", "name", "contactPerson", "phoneNumber","address","createdAtUtc" ,"id" },
					new List<string>(),
					dto.Filters
				);
				filteredData = queryable.Where(expression);
				count = allSupplierDetails.total;
			}

			if (dto.Sorters.Count > 0)
			{
				var elem = dto.Sorters[0];
				var expression = $"x => {elem.Field.Pascalize()} {elem.Dir.ToUpper()}";

				if (filteredData is null)
				{
					filteredData = queryable.OrderBy(expression);
					count = allSupplierDetails.total;
				}
				else
				{
					filteredData = filteredData.OrderBy(expression);
				}
			}

			if (filteredData is null)
			{
				filteredData = queryable;
				count = allSupplierDetails.total;
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
			var model = new EditSupplierModel();
			model.Resolve(_scope);
			model = await model.GetSupplierByIdAsync(id);
			TempData["CreatedAtUtc"] = model.CreatedAtUtc;
			return View(model);
		}

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditSupplierModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			if (TempData.ContainsKey("CreatedAtUtc"))
			{
				model.CreatedAtUtc = (DateTime)TempData["CreatedAtUtc"];
			}

			if (!Directory.Exists(_imageDirectory))
			{
				Directory.CreateDirectory(_imageDirectory);
			}

			if (model.ImageFile != null && model.ImageFile.Length > 0)
			{
				var uniqueFileName = $"{Guid.NewGuid()}.jpg";
				var filePath = Path.Combine(_imageDirectory, uniqueFileName);

				using (var imageStream = model.ImageFile.OpenReadStream())
				using (var image = Image.FromStream(imageStream))
				{
					var resizedImage = ResizeImage(image, 250, 333); // Resize to 3:4 aspect ratio
					resizedImage.Save(filePath, ImageFormat.Jpeg);
				}
				model.PhotoUrl = $"/uploads/{uniqueFileName}";
			}

			model.UpdatedAtUtc = DateTime.UtcNow;
			var response = await model.BuildAdapter().AdaptToTypeAsync<EditSupplierDto>();
			model.Resolve(_scope);
			await model.EditSupplierAsync(response);
			return RedirectToAction("Get");
		}

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				var model = new DeleteSupplierModel();
				model.Resolve(_scope);
				await model.DeleteByIdAsync(id);
				return Json(new { success = true, message = "Supplier deleted successfully" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Error deleting Supplier: " + ex.Message });
			}
			return RedirectToAction("Get");
		}
	}
}