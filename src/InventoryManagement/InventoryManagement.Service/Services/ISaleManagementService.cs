using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Services
{
	public interface ISaleManagementService
	{
        
        Task CreateSaleOrderAsync(CreateSaleOrderDto dto);
		Task<(IList<SaleOrdersDto> data, int total, int totalDisplay)> GetSaleOrdersAsync(int pageIndex, int pageSize);
        Task<IList<SaleOrdersDto>> GetAllSaleOrderAsync();
        Task DeleteByIdAsync(Guid id);
		Task<SaleOrder> GetSaleOrderByIdAsync(Guid id);
		Task EditSaleOrderAsync(EditSaleOrderDto dto);
	}
}
