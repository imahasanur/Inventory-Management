using Autofac;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.Drawing.Printing;

namespace InventoryManagement.Presentation.Models
{
	public class CategoriesModel
	{
		private ILifetimeScope _scope;
		private ICategoryManagementService _categoryManagementService;

		public string EndpointUrl { get; init; }

		public CategoriesModel() { }

		public CategoriesModel(ICategoryManagementService categoryManagementService)
		{
			_categoryManagementService = categoryManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_categoryManagementService = _scope.Resolve<ICategoryManagementService>();
		}

		public async Task<(IList<CategoriesDto> data, int total, int totalDisplay)> GetCategoriesAsync(int pageIndex, int pageSize)
		{
			return await _categoryManagementService.GetCategoriesAsync(pageIndex,pageSize);
		}

		public async Task<IList<CategoriesDto>> GetAllCategoryAsync()
		{
			return await _categoryManagementService.GetAllCategoryAsync();
		}
	}
}
