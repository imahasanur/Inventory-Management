using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Services
{
	
	public interface ITransactionManagementService
	{
		Task CreateTransactionAsync(CreateTransactionDto dto);
		Task<(IList<TransactionsDto> data, int total, int totalDisplay)> GetTransactionsAsync(int pageIndex, int pageSize);
		Task<IList<TransactionsDto>> GetAllTransactionAsync();
        Task DeleteByIdAsync(Guid id);
		Task DeleteByTransactionProductIdAsync(Guid id);
		Task<Transaction> GetTransactionByIdAsync(Guid id);
		//Task EditProductAsync(EditProductDto dto);
	}
}
