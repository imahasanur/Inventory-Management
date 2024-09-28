using Autofac;
using InventoryManagement.Service.Services;

namespace InventoryManagement.Presentation.Models
{
	public class DeleteSupplierModel
	{
		private ILifetimeScope _scope;
		private ISupplierManagementService _supplierManagementService;

		public DeleteSupplierModel() { }

		public DeleteSupplierModel(ISupplierManagementService supplierManagementService)
		{
			_supplierManagementService = supplierManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_supplierManagementService = _scope.Resolve<ISupplierManagementService>();
		}
		public async Task DeleteByIdAsync(Guid id)
		{
			await _supplierManagementService.DeleteByIdAsync(id);
		}
	}
}
