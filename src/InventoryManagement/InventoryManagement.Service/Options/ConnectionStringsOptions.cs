using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Options
{
	public record ConnectionStringsOptions
	{
		public const string SectionName = "ConnectionStrings";
		[Required] public required string DbCon { get; init; }
	}
}
