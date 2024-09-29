using Autofac;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.Drawing.Printing;

namespace InventoryManagement.Presentation.Models
{
	public class PurchaseOrdersModel
    {
        private ILifetimeScope _scope;
        private IPurchaseManagementService _purchaseManagementService;

        public string EndpointUrl { get; init; }

		public PurchaseOrdersModel() { }

		public PurchaseOrdersModel(IPurchaseManagementService purchaseManagementService)
		{
            _purchaseManagementService = purchaseManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
            _purchaseManagementService = _scope.Resolve<IPurchaseManagementService>();
		}

		public async Task<(IList<PurchaseOrdersDto> data, int total, int totalDisplay)> GetPurchaseOrdersAsync(int pageIndex, int pageSize)
		{
			return await _purchaseManagementService.GetPurchaseOrdersAsync(pageIndex,pageSize);
		}

		//public async Task<IList<CategoriesDto>> GetAllCategoryAsync()
		//{
		//	return await _productManagementService.GetAllCategoryAsync();
		//}
	}
}
