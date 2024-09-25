using DotNetEnv;
using InventoryManagement.Presentation.Data;
using InventoryManagement.Service.Options;
using InventoryManagement.Data.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Serilog.Events;
using System.Reflection;
using InventoryManagement.Data;
using InventoryManagement.Service;
using InventoryManagement.Presentation;

Env.Load();

var configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json", optional: false)
	.AddEnvironmentVariables()
	.Build();

Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(configuration)
	.CreateBootstrapLogger();


try
{
	var builder = WebApplication.CreateBuilder(args);

	builder.Configuration
		.AddJsonFile("appsettings.json", optional: false)
		.AddEnvironmentVariables();

	builder.Services.BindAndValidateOptions<ConnectionStringsOptions>(ConnectionStringsOptions.SectionName);

	builder.Host.UseSerilog((ctx, lc) => lc
	.MinimumLevel.Debug()
	.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
	.Enrich.FromLogContext()
	.ReadFrom.Configuration(builder.Configuration));

	// Add services to the container.
	var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__DbCon");
	var migrationAssembly = Assembly.GetExecutingAssembly().FullName;
	builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
	builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
	{
		containerBuilder.RegisterModule(new ServiceModule());
		containerBuilder.RegisterModule(new DataModule(connectionString, migrationAssembly));
		containerBuilder.RegisterModule(new PresentationModule());
	});

	// Add services to the container.
	builder.Services.AddDbContext<ApplicationDbContext>(options =>
		options.UseSqlServer(connectionString,
		(m) => m.MigrationsAssembly(migrationAssembly)));

	builder.Services.AddDatabaseDeveloperPageExceptionFilter();

	builder.Services.AddControllersWithViews();

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseMigrationsEndPoint();
	}
	else
	{
		app.UseExceptionHandler("/Home/Error");
		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseHsts();
	}

	app.UseHttpsRedirection();
	app.UseStaticFiles();

	app.UseRouting();

	app.UseAuthorization();

	app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");

	app.Run();

}
catch (Exception ex)
{
	Log.Fatal(ex, "Failed to start the application.");
}
finally
{
	Log.CloseAndFlush();
}
