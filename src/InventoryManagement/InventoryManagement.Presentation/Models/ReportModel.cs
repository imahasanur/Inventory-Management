using Autofac;
using InventoryManagement.Service.Dto;
using InventoryManagement.Service.Services;
using System.Drawing.Printing;

namespace InventoryManagement.Presentation.Models
{
	public class ReportModel
	{
        private ILifetimeScope _scope;
        private IPurchaseManagementService _purchaseManagementService;

        public string EndpointUrl { get; init; }

		public ReportModel() { }

		public ReportModel(IPurchaseManagementService purchaseManagementService)
		{
            _purchaseManagementService = purchaseManagementService;
		}

		public void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
            _purchaseManagementService = _scope.Resolve<IPurchaseManagementService>();
		}

	}
}
