﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.Domain.Entities
{
	public interface ITimeStamp
	{
		public DateTime CreatedAtUtc { get; set; }
		public DateTime? UpdatedAtUtc { get; set; }
	}
}
