using Autofac;
using InventoryManagement.Service.Services;

namespace InventoryManagement.Presentation.Models
{
	public class DeletePurchaseOrderModel
    {
        private ILifetimeScope _scope;
        private IPurchaseManagementService _purchaseManagementService;

        public DeletePurchaseOrderModel() { }

		public DeletePurchaseOrderModel(IPurchaseManagementService purchaseManagementService)
		{
            _purchaseManagementService = purchaseManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
            _purchaseManagementService = _scope.Resolve<IPurchaseManagementService>();
		}
		public async Task DeleteByIdAsync(Guid id)
		{
			await _purchaseManagementService.DeleteByIdAsync(id);
		}
	}
}
