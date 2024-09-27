using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Domain.Repositories;
using InventoryManagement.Service.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventoryManagement.Data.Repositories
{
	public class CategoryRepository : Repository<Category, Guid>, ICategoryRepository
	{
		public CategoryRepository(IApplicationDbContext context) : base((DbContext)context)
		{
		}

		public async Task<(IList<Category>data, int total, int totalDisplay)> GetCategoriesAsync(int pageIndex, int pageSize)
		{
			return await GetDynamicAsync(null, null, null, pageIndex, pageSize, true);
		}

		public async Task CreateCategoryAsync(CreateCategoryDto dto)
		{
			var category = await dto.BuildAdapter().AdaptToTypeAsync<Category>();
			await AddAsync(category);
		}

		public async Task<Category> GetCategoryByIdAsync(Guid id)
		{
			return await GetByIdAsync(id);
		}

		public async Task EditCategoryAsync(Category categoryEntityObj)
		{
			await EditAsync(categoryEntityObj);
		}

		public async Task DeleteCategoryByIdAsync(Guid id)
		{
			Expression<Func<Category, bool>> expression = x => x.Id == id;
			await RemoveAsync(expression);
		}

	}
}
