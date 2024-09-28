using Autofac;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.Drawing.Printing;

namespace InventoryManagement.Presentation.Models
{
	public class SuppliersModel
	{
		private ILifetimeScope _scope;
		private ISupplierManagementService _supplierManagementService;

		public string EndpointUrl { get; init; }

		public SuppliersModel() { }

		public SuppliersModel(ISupplierManagementService supplierManagementService)
		{
			_supplierManagementService = supplierManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_supplierManagementService = _scope.Resolve<ISupplierManagementService>();
		}

		public async Task<(IList<SuppliersDto> data, int total, int totalDisplay)> GetSuppliersAsync(int pageIndex, int pageSize)
		{
			return await _supplierManagementService.GetSuppliersAsync(pageIndex,pageSize);
		}

	}
}
