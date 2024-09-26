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

        }
	}
}
