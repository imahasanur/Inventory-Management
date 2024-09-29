using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Domain.Repositories
{
	public interface ITransactionRepository : IRepositoryBase<Transaction,Guid>
	{
		Task<(IList<Transaction> data, int total, int totalDisplay)> GetTransactionsAsync(int pageIndex, int pageSize);
		Task<IList<Transaction>> GetAllTransactionAsync();
        Task CreateTransactionAsync(CreateTransactionDto dto);
		Task<Transaction> GetTransactionByIdAsync(Guid id);
		//Task EditProductAsync(Product entityObj);
		Task DeleteByIdAsync(Guid id);
		Task DeleteByTransactionProductIdAsync(Guid id);
	}
}
