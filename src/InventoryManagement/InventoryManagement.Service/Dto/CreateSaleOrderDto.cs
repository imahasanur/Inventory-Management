﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Dto
{
	public record CreateSaleOrderDto
	{
        public required Guid ProductId { get; set; }
        public required string ProductName { get; set; }
        public required int Quantity { get; set; }
        public required int UnitPrice { get; set; }
        public required int TotalAmount { get; set; }
        public required string Status { get; set; }
        public required string User { get; set; }
        public required DateTime CreatedAtUtc { get; set; }
    }
}
