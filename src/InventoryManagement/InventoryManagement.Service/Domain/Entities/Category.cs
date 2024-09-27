using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Domain.Entities
{
	public class Category : IEntity<Guid>, ITimeStamp
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAtUtc { get; set; }
		public DateTime? UpdatedAtUtc { get; set; }
	}
}
