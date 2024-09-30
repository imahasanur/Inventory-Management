using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Dto
{
	public record CreateTransactionDto
    {
        public required Guid ProductId { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public Guid? SaleOrderId { get; set; }
        public required int Quantity { get; set; }
        public required int TotalAmount { get; set; }
        public required string TransactionType { get; set; }
        public required string User { get; set; }
        public required DateTime CreatedAtUtc { get; set; }
    }
}
