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
	public class SupplierManagementService : ISupplierManagementService
	{
		private readonly IApplicationUnitOfWork _unitOfWork;

		public SupplierManagementService(IApplicationUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<(IList<SuppliersDto> data, int total, int totalDisplay)> GetSuppliersAsync(int pageIndex, int pageSize)
		{
			var entityList = await _unitOfWork.SupplierRepository.GetSuppliersAsync(pageIndex, pageSize);
			var result = await entityList.data.BuildAdapter().AdaptToTypeAsync<List<SuppliersDto>>();
			return (result, entityList.total, entityList.totalDisplay);
		}

        public async Task<IList<SuppliersDto>> GetAllSupplierAsync()
        {
            var entityList = await _unitOfWork.SupplierRepository.GetAllSupplierAsync();
            var result = await entityList.BuildAdapter().AdaptToTypeAsync<List<SuppliersDto>>();
            return result;
        }

        public async Task CreateSupplierAsync(CreateSupplierDto dto)
		{
			await _unitOfWork.SupplierRepository.CreateSupplierAsync(dto);
			await _unitOfWork.SaveAsync();
		}

		public async Task<Supplier> GetSupplierByIdAsync(Guid id)
		{
			var entity = await _unitOfWork.SupplierRepository.GetSupplierByIdAsync(id);
			return entity;
		}

		public async Task EditSupplierAsync(EditSupplierDto dto)
		{
			var entityObj = await dto.BuildAdapter().AdaptToTypeAsync<Supplier>();
			await _unitOfWork.SupplierRepository.EditSupplierAsync(entityObj);
			await _unitOfWork.SaveAsync();
		}

		public async Task DeleteByIdAsync(Guid id)
		{
			await _unitOfWork.SupplierRepository.DeleteByIdAsync(id);
			await _unitOfWork.SaveAsync();
		}
	}
}
