using Autofac;
using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Presentation.Models
{
	public class CreateSaleOrderModel
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
	
        public int? TotalAmount { get; set; }

        public string? Status { get; set; }
	
        public DateTime CreatedAtUtc { get; set; }

		public IList<ProductsDto>? Products { get; set; }

        public CreateSaleOrderModel() { }

		public CreateSaleOrderModel(ISaleManagementService saleManagementService)
		{
            _saleManagementService = saleManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
            _saleManagementService = _scope.Resolve<ISaleManagementService>();
		}

		public async Task CreateSaleOrderAsync(CreateSaleOrderDto dto)
		{
			await _saleManagementService.CreateSaleOrderAsync(dto);
		}
	}
}
