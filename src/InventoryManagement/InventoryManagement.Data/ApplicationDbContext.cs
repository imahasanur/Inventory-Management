﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Data;
using InventoryManagement.Data.Membership;
using InventoryManagement.Service.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,
        ApplicationRole, Guid,
        ApplicationUserClaim, ApplicationUserRole,
        ApplicationUserLogin, ApplicationRoleClaim,
        ApplicationUserToken>,
        IApplicationDbContext
    {

        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public ApplicationDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString,
                    x => x.MigrationsAssembly(_migrationAssembly));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
			builder.Entity<Product>()
	        .HasOne<Category>()
	        .WithMany()
	        .HasForeignKey(x => x.CategoryId);

            builder.Entity<PurchaseOrder>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId);

            builder.Entity<PurchaseOrder>()
            .HasOne<Supplier>()
            .WithMany()
            .HasForeignKey(x => x.SupplierId);

            builder.Entity<Transaction>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId);

            builder.Entity<SaleOrder>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId);

			//seed an admin
			var adminUserId = new Guid("D88D9D62-162D-4044-BE26-FC89C796B4E7");
			var passwordHasher = new PasswordHasher<ApplicationUser>();
			var adminUser = new ApplicationUser
			{
				Id = adminUserId,
				UserName = "admin@gmail.com",
				NormalizedUserName = "ADMIN@GMAIL.COM",
				Email = "admin@gmail.com",
				NormalizedEmail = "ADMIN@GMAIL.COM",
				EmailConfirmed = true,
				SecurityStamp = string.Empty,
				FirstName = "Admin",
				LastName = "User",
				FullName = "Admin User",
				CreatedAtUtc = DateTime.UtcNow
			};

			// Hash the password "1234567"
			adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "1234567");

			builder.Entity<ApplicationUser>().HasData(adminUser);

			// Seed claims for the admin user
			builder.Entity<ApplicationUserClaim>().HasData(
				new ApplicationUserClaim
				{
					Id = 1,
					UserId = adminUserId,
					ClaimType = "role",
					ClaimValue = "admin"
				},
				new ApplicationUserClaim
				{
					Id = 2,
					UserId = adminUserId,
					ClaimType = "role",
					ClaimValue = "user"
				}
			);

			base.OnModelCreating(builder);
        }
		public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
		public DbSet<Supplier> Supplier { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<SaleOrder> SaleOrder { get ; set; }
    }
}
