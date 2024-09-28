using Autofac;
using Mapster;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Presentation.Models
{
	public class EditSupplierModel
	{
		private ILifetimeScope _scope;
		private ISupplierManagementService _supplierManagementService;

		public Guid Id { get; init; }

		[Required(ErrorMessage = "Supplier Name is required")]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		[Required(ErrorMessage = "Contact person Name is required")]
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
		public DateTime? UpdatedAtUtc { get; set; }


		public EditSupplierModel() { }

		public EditSupplierModel(ISupplierManagementService supplierManagementService)
		{
			_supplierManagementService = supplierManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_supplierManagementService = _scope.Resolve<ISupplierManagementService>();
		}

		public async Task<EditSupplierModel> GetSupplierByIdAsync(Guid id)
		{
			var response = await _supplierManagementService.GetSupplierByIdAsync(id);
			var model = await response.BuildAdapter().AdaptToTypeAsync<EditSupplierModel>();
			return model;
		}

		public async Task EditSupplierAsync(EditSupplierDto dto)
		{
			await _supplierManagementService.EditSupplierAsync(dto);
		}
	}
}
