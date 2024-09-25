using DotNetEnv;
using InventoryManagement.Presentation.Data;
using InventoryManagement.Service.Options;
using InventoryManagement.Data.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
	var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__DbCon");
	builder.Services.AddDbContext<ApplicationDbContext>(options =>
		options.UseSqlServer(connectionString));
	builder.Services.AddDatabaseDeveloperPageExceptionFilter();

	builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
		.AddEntityFrameworkStores<ApplicationDbContext>();
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
	app.MapRazorPages();

	Log.Information("Application started");

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
