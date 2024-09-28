using Autofac;
using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Presentation.Models
{
	public class CreateProductModel
	{
		private ILifetimeScope _scope;
		private IProductManagementService _productManagementService;

		public Guid Id { get; init; }

		[Required(ErrorMessage ="Product Name is required")]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		[Required(ErrorMessage ="Product Description is required")]
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
		public IFormFile? ImageFile { get; set; }
		public IList<CategoriesDto>? Categories { get; set; }

		public CreateProductModel() { }

		public CreateProductModel(IProductManagementService productManagementService)
		{
			_productManagementService = productManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_productManagementService = _scope.Resolve<IProductManagementService>();
		}

		public async Task CreateProductAsync(CreateProductDto dto)
		{
			await _productManagementService.CreateProductAsync(dto);
		}
	}
}
