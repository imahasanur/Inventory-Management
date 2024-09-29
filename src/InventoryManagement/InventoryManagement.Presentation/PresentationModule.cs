using Autofac;
using InventoryManagement.Presentation.Models;

namespace InventoryManagement.Presentation
{
	public class PresentationModule:Module
	{
		protected override void Load(ContainerBuilder builder)
		{
            builder.RegisterType<RegistrationModel>().AsSelf();
            builder.RegisterType<LoginModel>().AsSelf();
			builder.RegisterType<CreateCategoryModel>().AsSelf();
			builder.RegisterType<EditCategoryModel>().AsSelf();
			builder.RegisterType<DeleteCategoryModel>().AsSelf();
			builder.RegisterType<CreateProductModel>().AsSelf();
			builder.RegisterType<EditProductModel>().AsSelf();
			builder.RegisterType<CreateSupplierModel>().AsSelf();
			builder.RegisterType<EditSupplierModel>().AsSelf();
            builder.RegisterType<CreatePurchaseOrderModel>().AsSelf();
            builder.RegisterType<EditPurchaseOrderModel>().AsSelf();
			builder.RegisterType<CreateTransactionModel>().AsSelf();


		}
    }
}
