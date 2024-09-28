using Autofac;
using Mapster;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Presentation.Models
{
	public class EditProductModel
	{
		private ILifetimeScope _scope;
		private IProductManagementService _productManagementService;

		public Guid Id { get; init; }

		[Required(ErrorMessage = "Product Name is required")]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		[Required(ErrorMessage = "Product Description is required")]
		[DataType(DataType.Text)]
		public string Description { get; set; }

		public string? PhotoUrl { get; set; }

		[Required(ErrorMessage = "Stock Level is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Stock Level must be greater than zero")]
		public int StockLevel { get; set; }

		[Required(ErrorMessage = "Unit Price is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Unit Price must be greater than zero")]
		public int UnitPrice { get; set; }
		public Guid CategoryId { get; set; }

		[Required(ErrorMessage = "Category Name is required")]
		public string CategoryName { get; set; }
		public DateTime CreatedAtUtc { get; set; }
		public DateTime? UpdatedAtUtc { get; set; }
		public IFormFile? ImageFile { get; set; }
		public IList<CategoriesDto>? Categories { get; set; }

		public EditProductModel() { }

		public EditProductModel(IProductManagementService productManagementService)
		{
			_productManagementService = productManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_productManagementService = _scope.Resolve<IProductManagementService>();
		}

		public async Task<EditProductModel> GetProductByIdAsync(Guid id)
		{
			var response = await _productManagementService.GetProductByIdAsync(id);
			var model = await response.BuildAdapter().AdaptToTypeAsync<EditProductModel>();
			return model;
		}

		public async Task EditProductAsync(EditProductDto dto)
		{
			await _productManagementService.EditProductAsync(dto);
		}
	}
}
