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
	public class PurchaseOrderRepository : Repository<PurchaseOrder, Guid>, IPurchaseOrderRepository
    {
		public PurchaseOrderRepository(IApplicationDbContext context) : base((DbContext)context)
		{
		}

		public async Task<(IList<PurchaseOrder> data, int total, int totalDisplay)> GetPurchaseOrdersAsync(int pageIndex, int pageSize)
		{
			return await GetDynamicAsync(null, null, null, pageIndex, pageSize, true);
		}

		public async Task CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto)
		{
			var purchaseOrder = await dto.BuildAdapter().AdaptToTypeAsync<PurchaseOrder>();
			await AddAsync(purchaseOrder);
		}

		public async Task<PurchaseOrder> GetPurchaseOrderByIdAsync(Guid id)
		{
			return await GetByIdAsync(id);
		}

		public async Task EditPurchaseOrderAsync(PurchaseOrder entityObj)
		{
			await EditAsync(entityObj);
		}

		public async Task DeleteByIdAsync(Guid id)
		{
			Expression<Func<PurchaseOrder, bool>> expression = x => x.Id == id;
			await RemoveAsync(expression);
		}

	}
}
