using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Services
{
	public interface ISupplierManagementService
	{
        
        Task CreateSupplierAsync(CreateSupplierDto dto);
		Task<(IList<SuppliersDto> data, int total, int totalDisplay)> GetSuppliersAsync(int pageIndex, int pageSize);
        Task<IList<SuppliersDto>> GetAllSupplierAsync();
        Task DeleteByIdAsync(Guid id);
		Task<Supplier> GetSupplierByIdAsync(Guid id);
		Task EditSupplierAsync(EditSupplierDto dto);
	}
}
