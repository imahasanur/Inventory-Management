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
	public class SaleManagementService : ISaleManagementService
    {
		private readonly IApplicationUnitOfWork _unitOfWork;

		public SaleManagementService(IApplicationUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<(IList<SaleOrdersDto> data, int total, int totalDisplay)> GetSaleOrdersAsync(int pageIndex, int pageSize)
		{
			var entityList = await _unitOfWork.SaleOrderRepository.GetSaleOrdersAsync(pageIndex, pageSize);
			var result = await entityList.data.BuildAdapter().AdaptToTypeAsync<List<SaleOrdersDto>>();
			return (result, entityList.total, entityList.totalDisplay);
		}

        public async Task<IList<SaleOrdersDto>> GetAllSaleOrderAsync()
        {
            var entityList = await _unitOfWork.SaleOrderRepository.GetAllSaleOrderAsync();
            var result = await entityList.BuildAdapter().AdaptToTypeAsync<List<SaleOrdersDto>>();
            return result;
        }
        
        public async Task CreateSaleOrderAsync(CreateSaleOrderDto dto)
		{
			await _unitOfWork.SaleOrderRepository.CreateSaleOrderAsync(dto);
			await _unitOfWork.SaveAsync();
		}

		public async Task<SaleOrder> GetSaleOrderByIdAsync(Guid id)
		{
			var entity = await _unitOfWork.SaleOrderRepository.GetSaleOrderByIdAsync(id);
			return entity;
		}

		public async Task EditSaleOrderAsync(EditSaleOrderDto dto)
		{
			var entityObj = await dto.BuildAdapter().AdaptToTypeAsync<SaleOrder>();
			await _unitOfWork.SaleOrderRepository.EditSaleOrderAsync(entityObj);
			await _unitOfWork.SaveAsync();
		}

		public async Task DeleteByIdAsync(Guid id)
		{
			await _unitOfWork.SaleOrderRepository.DeleteByIdAsync(id);
			await _unitOfWork.SaveAsync();
		}
	}
}
