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
	public class CategoryManagementService: ICategoryManagementService
	{
		private readonly IApplicationUnitOfWork _unitOfWork;

		public CategoryManagementService(IApplicationUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<(IList<CategoriesDto>data, int total, int totalDisplay)> GetCategoriesAsync(int pageIndex, int pageSize)
		{
			var categoryEntityList = await _unitOfWork.CategoryRepository.GetCategoriesAsync(pageIndex,pageSize);
			var result = await categoryEntityList.data.BuildAdapter().AdaptToTypeAsync<List<CategoriesDto>>();
			return (result, categoryEntityList.total, categoryEntityList.totalDisplay);
		}

		public async Task CreateCategoryAsync(CreateCategoryDto dto)
		{
			//var employee = await dto.BuildAdapter().AdaptToTypeAsync<Category>();
			await _unitOfWork.CategoryRepository.CreateCategoryAsync(dto);
			await _unitOfWork.SaveAsync();

		}

		public async Task<Category> GetCategoryByIdAsync(Guid id)
		{
			var categoryEntity = await _unitOfWork.CategoryRepository.GetCategoryByIdAsync(id);
			return categoryEntity;
		}

		public async Task EditCategoryAsync(EditCategoryDto dto)
		{
			var categoryEntityObj = await dto.BuildAdapter().AdaptToTypeAsync<Category>();
			await _unitOfWork.CategoryRepository.EditCategoryAsync(categoryEntityObj);
			await _unitOfWork.SaveAsync();

		}

		public async Task DeleteCategoryByIdAsync(Guid id)
		{
			await _unitOfWork.CategoryRepository.DeleteCategoryByIdAsync(id);
			await _unitOfWork.SaveAsync();
		}
	}
}
