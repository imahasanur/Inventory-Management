using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection BindAndValidateOptions<TOptions>(this IServiceCollection services,
			string sectionName) where TOptions : class
		{
			
			services.AddOptions<TOptions>()
				.BindConfiguration(sectionName)
				.ValidateDataAnnotations()
				.ValidateOnStart();
			return services;
		}
	}
}
