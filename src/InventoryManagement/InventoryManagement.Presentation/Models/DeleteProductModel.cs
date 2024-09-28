using Autofac;
using InventoryManagement.Service.Services;

namespace InventoryManagement.Presentation.Models
{
	public class DeleteProductModel
	{
		private ILifetimeScope _scope;
		private IProductManagementService _productManagementService;

		public DeleteProductModel() { }

		public DeleteProductModel(IProductManagementService productManagementService)
		{
			_productManagementService = productManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_productManagementService = _scope.Resolve<IProductManagementService>();
		}
		public async Task DeleteByIdAsync(Guid id)
		{
			await _productManagementService.DeleteByIdAsync(id);
		}
	}
}
