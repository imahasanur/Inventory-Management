using Autofac;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Presentation.Models
{
	public class CreateCategoryModel
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

		public CreateCategoryModel() { }

		public CreateCategoryModel(ICategoryManagementService categoryManagementService)
		{
			_categoryManagementService = categoryManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_categoryManagementService = _scope.Resolve<ICategoryManagementService>();
		}

		public async Task CreateCategoryAsync(CreateCategoryDto dto)
		{
			await _categoryManagementService.CreateCategoryAsync(dto);
		}
	}
}
