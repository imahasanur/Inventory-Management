using Autofac;
using Mapster;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Presentation.Models
{
	public class EditSaleOrderModel
    {
        private ILifetimeScope _scope;
        private ISaleManagementService _saleManagementService;

        public Guid Id { get; init; }
        [Required]
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
        [Required(ErrorMessage = "Sale status is required")]
        [DataType(DataType.Text)]
        public string Status { get; set; }
        [Required]
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
        public string? User { get; set; }

        public IList<ProductsDto>? Products { get; set; }

        public EditSaleOrderModel() { }

        public EditSaleOrderModel(ISaleManagementService saleManagementService)
        {
            _saleManagementService = saleManagementService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _saleManagementService = _scope.Resolve<ISaleManagementService>();
        }

        public async Task<EditSaleOrderModel> GetSaleOrderByIdAsync(Guid id)
		{
			var response = await _saleManagementService.GetSaleOrderByIdAsync(id);
			var model = await response.BuildAdapter().AdaptToTypeAsync<EditSaleOrderModel>();
			return model;
		}

		public async Task EditSaleOrderAsync(EditSaleOrderDto dto)
		{
			await _saleManagementService.EditSaleOrderAsync(dto);
		}
	}
}
