using Autofac;
using InventoryManagement.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service
{
	public class ServiceModule:Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<CategoryManagementService>().As<ICategoryManagementService>().InstancePerLifetimeScope();
			builder.RegisterType<ProductManagementService>().As<IProductManagementService>().InstancePerLifetimeScope();
            builder.RegisterType<SupplierManagementService>().As<ISupplierManagementService>().InstancePerLifetimeScope();
            builder.RegisterType<PurchaseManagementService>().As<IPurchaseManagementService>().InstancePerLifetimeScope();
			builder.RegisterType<TransactionManagementService>().As<ITransactionManagementService>().InstancePerLifetimeScope();

		}
	}
}
