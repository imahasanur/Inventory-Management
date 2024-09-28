using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Domain.Repositories
{
	public interface ICategoryRepository:IRepositoryBase<Category,Guid>
	{
		Task<(IList<Category> data, int total, int totalDisplay)> GetCategoriesAsync(int pageIndex, int pageSize);
		Task CreateCategoryAsync(CreateCategoryDto dto);
		Task<Category> GetCategoryByIdAsync(Guid id);
		Task EditCategoryAsync(Category categoryEntityObj);
		Task DeleteCategoryByIdAsync(Guid id);
		Task<IList<Category>> GetAllCategoryAsync();
	}
}
