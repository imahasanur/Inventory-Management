using Autofac;
using InventoryManagement.Service.Services;

namespace InventoryManagement.Presentation.Models
{
	public class DeleteCategoryModel
	{
		private ILifetimeScope _scope;
		private ICategoryManagementService _categoryManagementService;

		public DeleteCategoryModel() { }

		public DeleteCategoryModel(ICategoryManagementService categoryManagementService)
		{
			_categoryManagementService = categoryManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_categoryManagementService = _scope.Resolve<ICategoryManagementService>();
		}

		public async Task DeleteCategoryByIdAsync(Guid id)
		{
			await _categoryManagementService.DeleteCategoryByIdAsync(id);
		}
	}
}
