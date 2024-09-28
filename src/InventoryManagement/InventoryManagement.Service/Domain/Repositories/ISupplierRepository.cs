using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Domain.Repositories
{
	public interface ISupplierRepository : IRepositoryBase<Supplier,Guid>
	{
		Task<(IList<Supplier> data, int total, int totalDisplay)> GetSuppliersAsync(int pageIndex, int pageSize);
		Task CreateSupplierAsync(CreateSupplierDto dto);
		Task<Supplier> GetSupplierByIdAsync(Guid id);
		Task EditSupplierAsync(Supplier entityObj);
		Task DeleteByIdAsync(Guid id);
	}
}
