using InventoryManagement.Service.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service
{
	public interface IApplicationUnitOfWork: IUnitOfWork
	{
		ICategoryRepository CategoryRepository { get; }
	}
}
