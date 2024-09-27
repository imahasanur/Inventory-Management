using InventoryManagement.Service;
using InventoryManagement.Service.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
		public ICategoryRepository CategoryRepository { get; private set; }
		public ApplicationUnitOfWork(ICategoryRepository categoryRepository,
            IApplicationDbContext dbContext) : base((DbContext)dbContext)
        {
            CategoryRepository = categoryRepository;
        }
    }
}
