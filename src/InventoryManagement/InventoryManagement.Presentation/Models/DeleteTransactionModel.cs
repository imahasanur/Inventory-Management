using Autofac;
using InventoryManagement.Service.Services;

namespace InventoryManagement.Presentation.Models
{
	public class DeleteTransactionModel
	{
		private ILifetimeScope _scope;
		private ITransactionManagementService _transactionManagementService;
		public DeleteTransactionModel() { }

		public DeleteTransactionModel(ITransactionManagementService transactionManagementService)
		{
			_transactionManagementService = transactionManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_transactionManagementService = _scope.Resolve<ITransactionManagementService>();
		}
		public async Task DeleteByIdAsync(Guid id)
		{
			await _transactionManagementService.DeleteByIdAsync(id);
		}

		public async Task DeleteByTransactionProductIdAsync(Guid id)
		{
			await _transactionManagementService.DeleteByTransactionProductIdAsync(id);
		}
	}
}
