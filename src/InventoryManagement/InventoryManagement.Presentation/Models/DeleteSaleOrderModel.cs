using Autofac;
using InventoryManagement.Service.Services;

namespace InventoryManagement.Presentation.Models
{
	public class DeleteSaleOrderModel
    {
        private ILifetimeScope _scope;
        private ISaleManagementService _saleManagementService;

        public DeleteSaleOrderModel() { }

		public DeleteSaleOrderModel(ISaleManagementService saleManagementService)
		{
            _saleManagementService = saleManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
            _saleManagementService = _scope.Resolve<ISaleManagementService>();
		}
		public async Task DeleteByIdAsync(Guid id)
		{
			await _saleManagementService.DeleteByIdAsync(id);
		}
	}
}
