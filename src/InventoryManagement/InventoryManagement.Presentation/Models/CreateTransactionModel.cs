using Autofac;
using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Presentation.Models
{
	public class CreateTransactionModel
	{
		private ILifetimeScope _scope;
		private ITransactionManagementService _transactionManagementService;

		public Guid Id { get; init; }
		[Required]
		public Guid ProductId { get; set; }
		public Guid? PurchaseOrderId { get; set; }
		public Guid? SaleOrderId { get; set; }
		[Required]
		public int Quantity { get; set; }
		[Required]
		public int TotalAmount { get; set; }
		[Required]
		public string TransactionType { get; set; }
		[Required]
		public DateTime CreatedAtUtc { get; set; }

		public CreateTransactionModel() { }

		public CreateTransactionModel(ITransactionManagementService transactionManagementService)
		{
			_transactionManagementService = transactionManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_transactionManagementService = _scope.Resolve<ITransactionManagementService>();
		}
		
		public async Task CreateTransactionAsync(CreateTransactionDto dto)
		{
			await _transactionManagementService.CreateTransactionAsync(dto);
		}
	}
}
