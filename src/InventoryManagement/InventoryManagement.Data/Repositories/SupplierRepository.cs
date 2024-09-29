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
	public class SupplierRepository : Repository<Supplier, Guid>, ISupplierRepository
	{
		public SupplierRepository(IApplicationDbContext context) : base((DbContext)context)
		{
		}

		public async Task<(IList<Supplier> data, int total, int totalDisplay)> GetSuppliersAsync(int pageIndex, int pageSize)
		{
			return await GetDynamicAsync(null, null, null, pageIndex, pageSize, true);
		}

        public async Task<IList<Supplier>> GetAllSupplierAsync()
        {
            return await GetAsync(null, null, null, true);
        }
        
        public async Task CreateSupplierAsync(CreateSupplierDto dto)
		{
			var supplier = await dto.BuildAdapter().AdaptToTypeAsync<Supplier>();
			await AddAsync(supplier);
		}

		public async Task<Supplier> GetSupplierByIdAsync(Guid id)
		{
			return await GetByIdAsync(id);
		}

		public async Task EditSupplierAsync(Supplier entityObj)
		{
			await EditAsync(entityObj);
		}

		public async Task DeleteByIdAsync(Guid id)
		{
			Expression<Func<Supplier, bool>> expression = x => x.Id == id;
			await RemoveAsync(expression);
		}

	}
}
