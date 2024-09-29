using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Domain.Entities
{
	public class Transaction : IEntity<Guid>, ITimeStamp
	{
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public Guid? PurchaseOrderId { get; set; }
		public Guid? SaleOrderId { get; set; }
		public int Quantity { get; set; }
		public int TotalAmount { get; set; }
        public string TransactionType { get; set; }
		public DateTime CreatedAtUtc { get; set; }
		public DateTime? UpdatedAtUtc { get; set; }
	}
}
