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
	public class ProductManagementService : IProductManagementService
	{
		private readonly IApplicationUnitOfWork _unitOfWork;

		public ProductManagementService(IApplicationUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<(IList<ProductsDto> data, int total, int totalDisplay)> GetProductsAsync(int pageIndex, int pageSize)
		{
			var productEntityList = await _unitOfWork.ProductRepository.GetProductsAsync(pageIndex, pageSize);
			var result = await productEntityList.data.BuildAdapter().AdaptToTypeAsync<List<ProductsDto>>();
			return (result, productEntityList.total, productEntityList.totalDisplay);
		}

		public async Task CreateProductAsync(CreateProductDto dto)
		{
			await _unitOfWork.ProductRepository.CreateProductAsync(dto);
			await _unitOfWork.SaveAsync();
		}

		public async Task<Product> GetProductByIdAsync(Guid id)
		{
			var entity = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
			return entity;
		}

		public async Task EditProductAsync(EditProductDto dto)
		{
			var entityObj = await dto.BuildAdapter().AdaptToTypeAsync<Product>();
			await _unitOfWork.ProductRepository.EditProductAsync(entityObj);
			await _unitOfWork.SaveAsync();
		}

		public async Task DeleteByIdAsync(Guid id)
		{
			await _unitOfWork.ProductRepository.DeleteByIdAsync(id);
			await _unitOfWork.SaveAsync();
		}
	}
}
