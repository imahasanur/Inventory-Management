using Autofac;
using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Presentation.Models
{
	public class CreateSupplierModel
	{
		private ILifetimeScope _scope;
		private ISupplierManagementService _supplierManagementService;

		public Guid Id { get; init; }

		[Required(ErrorMessage ="Supplier Name is required")]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		[Required(ErrorMessage ="Contact person Name is required")]
		[DataType(DataType.Text)]
		public string ContactPerson { get; set; }

		[Required(ErrorMessage = "Phone Number is required")]
		[DataType(DataType.Text)]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "Address is required")]
		[DataType(DataType.Text)]
		public string Address { get; set; }
		public string? PhotoUrl { get; set; }
		public DateTime CreatedAtUtc { get; set; }
		public IFormFile? ImageFile { get; set; }

		public CreateSupplierModel() { }

		public CreateSupplierModel(ISupplierManagementService supplierManagementService)
		{
			_supplierManagementService = supplierManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_supplierManagementService = _scope.Resolve<ISupplierManagementService>();
		}

		public async Task CreateSupplierAsync(CreateSupplierDto dto)
		{
			await _supplierManagementService.CreateSupplierAsync(dto);
		}
	}
}
