using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Dto
{
	public record PurchaseOrderDto
    {
		public string? PhotoUrl { get; set; }
		public required string Name { get; set; }
		public required string Description { get; set; }
		public required int StockLevel { get; set; }
		public required int UnitPrice { get; set; }
		public required Guid CategoryId { get; set; }
		public required string CategoryName { get; set; }
		public required DateTime CreatedAtUtc { get; set; }
	}
}
