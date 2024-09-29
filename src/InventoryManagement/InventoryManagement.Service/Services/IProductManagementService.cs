using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Services
{
	public interface IProductManagementService
	{
		Task CreateProductAsync(CreateProductDto dto);
		Task<(IList<ProductsDto> data, int total, int totalDisplay)> GetProductsAsync(int pageIndex, int pageSize);
		Task<IList<ProductsDto>> GetAllProductAsync();
        Task DeleteByIdAsync(Guid id);
		Task<Product> GetProductByIdAsync(Guid id);
		Task EditProductAsync(EditProductDto dto);
	}
}
