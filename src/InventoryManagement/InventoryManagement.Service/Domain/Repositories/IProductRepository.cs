using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Domain.Repositories
{
	public interface IProductRepository:IRepositoryBase<Product,Guid>
	{
		Task<(IList<Product> data, int total, int totalDisplay)> GetProductsAsync(int pageIndex, int pageSize);
		Task<IList<Product>> GetAllProductAsync();
        Task CreateProductAsync(CreateProductDto dto);
		Task<Product> GetProductByIdAsync(Guid id);
		Task EditProductAsync(Product entityObj);
		Task DeleteByIdAsync(Guid id);
	}
}
