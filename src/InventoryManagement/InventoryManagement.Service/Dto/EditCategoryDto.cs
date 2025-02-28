﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Dto
{
	public record EditCategoryDto
	{
		public required Guid Id { get; set; }
		public required string Name { get; set; }
		public required string Description { get; set; }
		public required DateTime CreatedAtUtc { get; set; }
		public DateTime? UpdatedAtUtc { get; set; }
	}
}
