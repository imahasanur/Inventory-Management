using Autofac;
using Mapster;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Presentation.Models
{
	public class EditPurchaseOrderModel
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
        [Required]
        public int TotalAmount { get; set; }
        [Required(ErrorMessage = "Purchase status is required")]
        [DataType(DataType.Text)]
        public string Status { get; set; }
        [Required]
        public DateTime CreatedAtUtc { get; set; }
        public string? User { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }

        public IList<ProductsDto>? Products { get; set; }
        public IList<SuppliersDto>? Suppliers { get; set; }

        public EditPurchaseOrderModel() { }

        public EditPurchaseOrderModel(IPurchaseManagementService purchaseManagementService)
        {
            _purchaseManagementService = purchaseManagementService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _purchaseManagementService = _scope.Resolve<IPurchaseManagementService>();
        }

        public async Task<EditPurchaseOrderModel> GetPurchaseOrderByIdAsync(Guid id)
		{
			var response = await _purchaseManagementService.GetPurchaseOrderByIdAsync(id);
			var model = await response.BuildAdapter().AdaptToTypeAsync<EditPurchaseOrderModel>();
			return model;
		}

		public async Task EditPurchaseOrderAsync(EditPurchaseOrderDto dto)
		{
			await _purchaseManagementService.EditPurchaseOrderAsync(dto);
		}
	}
}
