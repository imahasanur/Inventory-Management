using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Services
{
	public interface ICategoryManagementService
	{
		Task CreateCategoryAsync(CreateCategoryDto dto);
		Task<(IList<CategoriesDto> data, int total, int totalDisplay)> GetCategoriesAsync(int pageIndex, int pageSize);
		Task DeleteCategoryByIdAsync(Guid id);
		Task<Category> GetCategoryByIdAsync(Guid id);
		Task EditCategoryAsync(EditCategoryDto dto);
		Task<IList<CategoriesDto>> GetAllCategoryAsync();
	}
}
