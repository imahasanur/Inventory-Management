using Autofac;
using Mapster;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Presentation.Models
{
	public class EditCategoryModel
	{
		private ILifetimeScope _scope;
		private ICategoryManagementService _categoryManagementService;

		public Guid Id { get; init; }
		[Required(ErrorMessage ="Category Name is required")]
		[DataType(DataType.Text)]
		public string Name { get; set; }
		[Required(ErrorMessage ="Category Description is required")]
		[DataType(DataType.Text)]
		public string Description { get; set; }
		public DateTime CreatedAtUtc { get; set; }
		public DateTime? UpdatedAtUtc { get; set; }

		public EditCategoryModel() { }

		public EditCategoryModel(ICategoryManagementService categoryManagementService)
		{
			_categoryManagementService = categoryManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_categoryManagementService = _scope.Resolve<ICategoryManagementService>();
		}

		public async Task<EditCategoryModel> GetCategoryByIdAsync(Guid id)
		{
			var response = await _categoryManagementService.GetCategoryByIdAsync(id);
			var model = await response.BuildAdapter().AdaptToTypeAsync<EditCategoryModel>();
			return model;
		}

		public async Task EditCategoryAsync(EditCategoryDto dto)
		{
			await _categoryManagementService.EditCategoryAsync(dto);
		}
	}
}
