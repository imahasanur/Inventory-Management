using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Dto
{
	public record CreateSupplierDto
	{
		public string? PhotoUrl { get; set; }
		public required string Name { get; set; }
		public required string ContactPerson { get; set; }
		public required string PhoneNumber { get; set; }
		public required string Address { get; set; }
		public required DateTime CreatedAtUtc { get; set; }
	}
}
