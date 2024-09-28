using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Domain.Entities
{
	public class Supplier : IEntity<Guid>, ITimeStamp
	{
		public Guid Id { get; set; }
		public string? PhotoUrl { get; set; }
		public string Name { get; set; }
		public string ContactPerson { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public DateTime CreatedAtUtc { get; set; }
		public DateTime? UpdatedAtUtc { get; set; }
	}
	
}
