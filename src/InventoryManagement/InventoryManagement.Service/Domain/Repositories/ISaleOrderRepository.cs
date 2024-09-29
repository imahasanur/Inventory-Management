using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Domain.Repositories
{
	public interface ISaleOrderRepository : IRepositoryBase<SaleOrder, Guid>
	{
		Task<(IList<SaleOrder> data, int total, int totalDisplay)> GetSaleOrdersAsync(int pageIndex, int pageSize);
		Task CreateSaleOrderAsync(CreateSaleOrderDto dto);
		Task<SaleOrder> GetSaleOrderByIdAsync(Guid id);
		Task EditSaleOrderAsync(SaleOrder entityObj);
		Task DeleteByIdAsync(Guid id);
	}
}
