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

namespace InventoryManagement.Presentation.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<ProductController> _logger;
        private readonly LinkGenerator _linkGenerator;
		private readonly string _imageDirectory = "wwwroot/uploads";
		public ProductController(ILifetimeScope scope,
            ILogger<ProductController> logger,
            LinkGenerator linkGenerator)
        {
            _scope = scope;
            _logger = logger;
            _linkGenerator = linkGenerator;
        }
        public async Task<IActionResult> Create()
        {
            var model = new CreateProductModel();
			var categoryModel = new CategoriesModel();
			categoryModel.Resolve(_scope);
			var result = await categoryModel.GetAllCategoryAsync();
            model.Resolve(_scope);
			model.Categories = result;
            
			return View(model);
        }

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateProductModel model)
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
				model.CreatedAtUtc = DateTime.UtcNow;
			}
			var productDto = await model.BuildAdapter().AdaptToTypeAsync<CreateProductDto>();
			model.Resolve(_scope);
			await model.CreateProductAsync(productDto);

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
		public async Task<IActionResult> Get()
		{
			var url = _linkGenerator.GetUriByAction(HttpContext, controller: "Product", action: "Get");
			if (url is null)
			{
				return View();
			}
			var viewModel = new ProductsModel { EndpointUrl = url };
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Get([FromBody] TabulatorQueryDto dto)
		{
			var model = new ProductsModel();
			model.Resolve(_scope);
			var allProductDetails = await model.GetProductsAsync(dto.Page, dto.Size);

			var dataSet = allProductDetails;
			var queryable = dataSet.data.AsQueryable();
			var count = 0;

			IQueryable<ProductsDto>? filteredData = null;

			if (dto.Filters.Count > 0)
			{
				var expression = ExpressionMaker(
					new List<string> { "id","photoUrl", "name", "description", "categoryName","categoryId","unitPrice","stockLevel", "createdAtUtc", "id" },
					new List<string>(),
					dto.Filters
				);
				filteredData = queryable.Where(expression);
				count = allProductDetails.total;
			}

			if (dto.Sorters.Count > 0)
			{
				var elem = dto.Sorters[0];
				var expression = $"x => {elem.Field.Pascalize()} {elem.Dir.ToUpper()}";

				if (filteredData is null)
				{
					filteredData = queryable.OrderBy(expression);
					count = allProductDetails.total;
				}
				else
				{
					filteredData = filteredData.OrderBy(expression);
				}
			}

			if (filteredData is null)
			{
				filteredData = queryable;
				count = allProductDetails.total;
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
					if (fieldInPascalCase == "Id" || fieldInPascalCase == "CategoryId")
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
			var model = new EditProductModel();
			model.Resolve(_scope);
			model = await model.GetProductByIdAsync(id);
			TempData["CreatedAtUtc"] = model.CreatedAtUtc;
			var categoryModel = new CategoriesModel();
			categoryModel.Resolve(_scope);
			var result = await categoryModel.GetAllCategoryAsync();
			model.Categories = result;
			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditProductModel model)
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
			var response = await model.BuildAdapter().AdaptToTypeAsync<EditProductDto>();
			model.Resolve(_scope);
			await model.EditProductAsync(response);
			return RedirectToAction("Get");
		}

		[HttpPost]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				var model = new DeleteProductModel();
				model.Resolve(_scope);
				await model.DeleteByIdAsync(id);
				return Json(new { success = true, message = "Product deleted successfully" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Error deleting Product: " + ex.Message });
			}
			return RedirectToAction("Get");
		}
	}
}