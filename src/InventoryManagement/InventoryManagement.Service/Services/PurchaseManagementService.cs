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
	public class PurchaseManagementService : IPurchaseManagementService
    {
		private readonly IApplicationUnitOfWork _unitOfWork;

		public PurchaseManagementService(IApplicationUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<(IList<PurchaseOrdersDto> data, int total, int totalDisplay)> GetPurchaseOrdersAsync(int pageIndex, int pageSize)
		{
			var entityList = await _unitOfWork.PurchaseOrderRepository.GetPurchaseOrdersAsync(pageIndex, pageSize);
			var result = await entityList.data.BuildAdapter().AdaptToTypeAsync<List<PurchaseOrdersDto>>();
			return (result, entityList.total, entityList.totalDisplay);
		}

		public async Task CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto)
		{
			await _unitOfWork.PurchaseOrderRepository.CreatePurchaseOrderAsync(dto);
			await _unitOfWork.SaveAsync();
		}

		public async Task<PurchaseOrder> GetPurchaseOrderByIdAsync(Guid id)
		{
			var entity = await _unitOfWork.PurchaseOrderRepository.GetPurchaseOrderByIdAsync(id);
			return entity;
		}

		public async Task EditPurchaseOrderAsync(EditPurchaseOrderDto dto)
		{
			var entityObj = await dto.BuildAdapter().AdaptToTypeAsync<PurchaseOrder>();
			await _unitOfWork.PurchaseOrderRepository.EditPurchaseOrderAsync(entityObj);
			await _unitOfWork.SaveAsync();
		}

		public async Task DeleteByIdAsync(Guid id)
		{
			await _unitOfWork.PurchaseOrderRepository.DeleteByIdAsync(id);
			await _unitOfWork.SaveAsync();
		}
	}
}
