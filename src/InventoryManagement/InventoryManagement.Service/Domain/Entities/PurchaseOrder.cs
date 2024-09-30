using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Domain.Entities
{
	public class PurchaseOrder : IEntity<Guid>, ITimeStamp
	{
		public Guid Id { get; set; }
		public Guid SupplierId { get; set; }
		public string SupplierName { get; set; }
        public Guid ProductId { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public int UnitPrice { get; set; }
		public int TotalAmount { get; set; }
		public string Status { get; set; }
        public string User { get; set; }
        public DateTime CreatedAtUtc { get; set; }
		public DateTime? UpdatedAtUtc { get; set; }
	}
}
