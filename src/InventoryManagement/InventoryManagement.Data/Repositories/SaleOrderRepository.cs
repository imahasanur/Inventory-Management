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
	public class SaleOrderRepository : Repository<SaleOrder, Guid>, ISaleOrderRepository
    {
		public SaleOrderRepository(IApplicationDbContext context) : base((DbContext)context)
		{
		}

		public async Task<(IList<SaleOrder> data, int total, int totalDisplay)> GetSaleOrdersAsync(int pageIndex, int pageSize)
		{
			return await GetDynamicAsync(null, null, null, pageIndex, pageSize, true);
		}

        public async Task<IList<SaleOrder>> GetAllSaleOrderAsync()
        {
            return await GetAsync(null, null, null, true);
        }
        
        public async Task CreateSaleOrderAsync(CreateSaleOrderDto dto)
		{
			var saleOrder = await dto.BuildAdapter().AdaptToTypeAsync<SaleOrder>();
			await AddAsync(saleOrder);
		}

		public async Task<SaleOrder> GetSaleOrderByIdAsync(Guid id)
		{
			return await GetByIdAsync(id);
		}

		public async Task EditSaleOrderAsync(SaleOrder entityObj)
		{
			await EditAsync(entityObj);
		}

		public async Task DeleteByIdAsync(Guid id)
		{
			Expression<Func<SaleOrder, bool>> expression = x => x.Id == id;
			await RemoveAsync(expression);
		}

	}
}
