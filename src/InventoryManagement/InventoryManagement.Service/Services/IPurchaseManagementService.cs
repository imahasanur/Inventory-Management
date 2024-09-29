using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Services
{
	public interface IPurchaseManagementService
    {
		Task CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto);
		Task<(IList<PurchaseOrdersDto> data, int total, int totalDisplay)> GetPurchaseOrdersAsync(int pageIndex, int pageSize);
		Task DeleteByIdAsync(Guid id);
		Task<PurchaseOrder> GetPurchaseOrderByIdAsync(Guid id);
		Task EditPurchaseOrderAsync(EditPurchaseOrderDto dto);
	}
}
