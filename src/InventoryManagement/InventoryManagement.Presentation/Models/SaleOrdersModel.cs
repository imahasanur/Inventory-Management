using Autofac;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.Drawing.Printing;

namespace InventoryManagement.Presentation.Models
{
	public class SaleOrdersModel
    {
        private ILifetimeScope _scope;
        private ISaleManagementService _saleManagementService;

        public string EndpointUrl { get; init; }

		public SaleOrdersModel() { }

		public SaleOrdersModel(ISaleManagementService saleManagementService)
		{
            _saleManagementService = saleManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
            _saleManagementService = _scope.Resolve<ISaleManagementService>();
		}

		public async Task<(IList<SaleOrdersDto> data, int total, int totalDisplay)> GetSaleOrdersAsync(int pageIndex, int pageSize)
		{
			return await _saleManagementService.GetSaleOrdersAsync(pageIndex,pageSize);
		}

	}
}
