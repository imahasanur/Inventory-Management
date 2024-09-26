using Autofac;
using InventoryManagement.Data.Membership;
using InventoryManagement.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data
{
	public class DataModule:Module
	{
		private readonly string _connectionString;
		private readonly string _migrationAssembly;
		public DataModule(string connectionString, string migrationAssembly)
		{
			_connectionString = connectionString;
			_migrationAssembly = migrationAssembly;
		}
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ApplicationDbContext>().AsSelf()
				.WithParameter("connectionString", _connectionString)
				.WithParameter("migrationAssembly", _migrationAssembly)
				.InstancePerLifetimeScope();

			builder.RegisterType<ApplicationDbContext>().As<IApplicationDbContext>()
				.WithParameter("connectionString", _connectionString)
				.WithParameter("migrationAssembly", _migrationAssembly)
				.InstancePerLifetimeScope();

			builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<TokenService>().As<ITokenService>()
            .InstancePerLifetimeScope();

            //base.Load(builder);
        }
	}
}
