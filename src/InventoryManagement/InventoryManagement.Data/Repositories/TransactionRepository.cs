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
	public class TransactionRepository : Repository<Transaction, Guid>, ITransactionRepository
	{
		public TransactionRepository(IApplicationDbContext context) : base((DbContext)context)
		{
		}

		public async Task<(IList<Transaction> data, int total, int totalDisplay)> GetTransactionsAsync(int pageIndex, int pageSize)
		{
			return await GetDynamicAsync(null, null, null, pageIndex, pageSize, true);
		}

		public async Task<IList<Transaction>> GetAllTransactionAsync()
		{
			return await GetAsync(null, null, null, true);
		}

		public async Task CreateTransactionAsync(CreateTransactionDto dto)
		{
			var transaction = await dto.BuildAdapter().AdaptToTypeAsync<Transaction>();
			await AddAsync(transaction);
		}

		public async Task<Transaction> GetTransactionByIdAsync(Guid id)
		{
			return await GetByIdAsync(id);
		}

		public async Task DeleteByIdAsync(Guid id)
		{
			Expression<Func<Transaction, bool>> expression = x => x.Id == id;
			await RemoveAsync(expression);
		}

		public async Task DeleteByTransactionProductIdAsync(Guid id)
		{
			Expression<Func<Transaction, bool>> expression = x => x.ProductId == id;
			await RemoveAsync(expression);
		}

	}
}
