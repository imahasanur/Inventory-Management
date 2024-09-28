using InventoryManagement.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data
{
    public interface IApplicationDbContext
    {
		public DbSet<Category> Category { get; set; }
		public DbSet<Product> Product { get; set; }
	}
}
