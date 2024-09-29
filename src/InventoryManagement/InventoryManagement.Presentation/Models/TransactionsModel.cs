using Autofac;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.Drawing.Printing;

namespace InventoryManagement.Presentation.Models
{
	public class TransactionsModel
	{
		private ILifetimeScope _scope;
		private ITransactionManagementService _transactionManagementService;

		public string EndpointUrl { get; init; }

		public TransactionsModel() { }

		public TransactionsModel(ITransactionManagementService transactionManagementService)
		{
			_transactionManagementService = transactionManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_transactionManagementService = _scope.Resolve<ITransactionManagementService>();
		}

		public async Task<(IList<TransactionsDto> data, int total, int totalDisplay)> GetTransactionsAsync(int pageIndex, int pageSize)
		{
			return await _transactionManagementService.GetTransactionsAsync(pageIndex,pageSize);
		}
		
		public async Task<IList<TransactionsDto>> GetAllTransactionAsync()
		{
			return await _transactionManagementService.GetAllTransactionAsync();
		}
	}
}
