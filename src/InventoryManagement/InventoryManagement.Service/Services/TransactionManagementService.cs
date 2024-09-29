using InventoryManagement.Service.Domain.Entities;
using InventoryManagement.Service.Dto;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventoryManagement.Service.Services
{
	public class TransactionManagementService : ITransactionManagementService
	{
		private readonly IApplicationUnitOfWork _unitOfWork;

		public TransactionManagementService(IApplicationUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		
		public async Task<(IList<TransactionsDto> data, int total, int totalDisplay)> GetTransactionsAsync(int pageIndex, int pageSize)
		{
			var transactionEntityList = await _unitOfWork.TransactionRepository.GetTransactionsAsync(pageIndex, pageSize);
			var result = await transactionEntityList.data.BuildAdapter().AdaptToTypeAsync<List<TransactionsDto>>();
			return (result, transactionEntityList.total, transactionEntityList.totalDisplay);
		}
		
        public async Task<IList<TransactionsDto>> GetAllTransactionAsync()
        {
            var transactionEntityList = await _unitOfWork.TransactionRepository.GetAllTransactionAsync();
            var result = await transactionEntityList.BuildAdapter().AdaptToTypeAsync<List<TransactionsDto>>();
            return result;
        }
        
        public async Task CreateTransactionAsync(CreateTransactionDto dto)
		{
			await _unitOfWork.TransactionRepository.CreateTransactionAsync(dto);
			await _unitOfWork.SaveAsync();
		}

		public async Task<Transaction> GetTransactionByIdAsync(Guid id)
		{
			var entity = await _unitOfWork.TransactionRepository.GetTransactionByIdAsync(id);
			return entity;
		}

		//public async Task EditProductAsync(EditProductDto dto)
		//{
		//	var entityObj = await dto.BuildAdapter().AdaptToTypeAsync<Product>();
		//	await _unitOfWork.ProductRepository.EditProductAsync(entityObj);
		//	await _unitOfWork.SaveAsync();
		//}

		public async Task DeleteByIdAsync(Guid id)
		{
			await _unitOfWork.TransactionRepository.DeleteByIdAsync(id);
			await _unitOfWork.SaveAsync();
		}

		public async Task DeleteByTransactionProductIdAsync(Guid id)
		{
			await _unitOfWork.TransactionRepository.DeleteByTransactionProductIdAsync(id);
			await _unitOfWork.SaveAsync();
		}
	}
}
