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
	public class ProductRepository : Repository<Product, Guid>, IProductRepository
	{
		public ProductRepository(IApplicationDbContext context) : base((DbContext)context)
		{
		}

		public async Task<(IList<Product> data, int total, int totalDisplay)> GetProductsAsync(int pageIndex, int pageSize)
		{
			return await GetDynamicAsync(null, null, null, pageIndex, pageSize, true);
		}

        public async Task<IList<Product>> GetAllProductAsync()
        {
            return await GetAsync(null, null, null, true);
        }
        
        public async Task CreateProductAsync(CreateProductDto dto)
		{
			var product = await dto.BuildAdapter().AdaptToTypeAsync<Product>();
			await AddAsync(product);
		}

		public async Task<Product> GetProductByIdAsync(Guid id)
		{
			return await GetByIdAsync(id);
		}

		public async Task EditProductAsync(Product entityObj)
		{
			await EditAsync(entityObj);
		}

		public async Task DeleteByIdAsync(Guid id)
		{
			Expression<Func<Product, bool>> expression = x => x.Id == id;
			await RemoveAsync(expression);
		}

	}
}
