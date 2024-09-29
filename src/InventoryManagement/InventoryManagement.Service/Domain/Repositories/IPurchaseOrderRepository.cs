using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Domain.Repositories
{
	public interface IPurchaseOrderRepository : IRepositoryBase<PurchaseOrder, Guid>
	{
		Task<(IList<PurchaseOrder> data, int total, int totalDisplay)> GetPurchaseOrdersAsync(int pageIndex, int pageSize);
		Task CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto);
		Task<PurchaseOrder> GetPurchaseOrderByIdAsync(Guid id);
		Task EditPurchaseOrderAsync(PurchaseOrder entityObj);
		Task DeleteByIdAsync(Guid id);
	}
}
