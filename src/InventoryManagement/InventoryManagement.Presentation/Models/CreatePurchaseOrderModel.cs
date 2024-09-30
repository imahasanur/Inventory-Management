using Autofac;
using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Presentation.Models
{
	public class CreatePurchaseOrderModel
    {
		private ILifetimeScope _scope;
		private IPurchaseManagementService _purchaseManagementService;

		public Guid Id { get; init; }
        [Required(ErrorMessage = "Supplier is required")]
        public Guid SupplierId { get; set; }
		[Required]
        public string SupplierName { get; set; }
        [Required(ErrorMessage = "Product is required")]
        public Guid ProductId { get; set; }
		[Required]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Unit Price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Unit Price must be greater than zero")]
        public int UnitPrice { get; set; }
        public int? TotalAmount { get; set; }
        public string? Status { get; set; }
        public string? User { get; set; }
        public DateTime CreatedAtUtc { get; set; }
		public IList<ProductsDto>? Products { get; set; }
        public IList<SuppliersDto>? Suppliers { get; set; }

        public CreatePurchaseOrderModel() { }

		public CreatePurchaseOrderModel(IPurchaseManagementService purchaseManagementService)
		{
            _purchaseManagementService = purchaseManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
            _purchaseManagementService = _scope.Resolve<IPurchaseManagementService>();
		}

		public async Task CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto)
		{
			await _purchaseManagementService.CreatePurchaseOrderAsync(dto);
		}
	}
}
