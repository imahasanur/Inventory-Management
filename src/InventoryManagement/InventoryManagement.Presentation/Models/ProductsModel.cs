using Autofac;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.Drawing.Printing;

namespace InventoryManagement.Presentation.Models
{
	public class ProductsModel
	{
		private ILifetimeScope _scope;
		private IProductManagementService _productManagementService;

		public string EndpointUrl { get; init; }

		public ProductsModel() { }

		public ProductsModel(IProductManagementService productManagementService)
		{
			_productManagementService = productManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_productManagementService = _scope.Resolve<IProductManagementService>();
		}

		public async Task<(IList<ProductsDto> data, int total, int totalDisplay)> GetProductsAsync(int pageIndex, int pageSize)
		{
			return await _productManagementService.GetProductsAsync(pageIndex,pageSize);
		}

		//public async Task<IList<CategoriesDto>> GetAllCategoryAsync()
		//{
		//	return await _productManagementService.GetAllCategoryAsync();
		//}
	}
}
