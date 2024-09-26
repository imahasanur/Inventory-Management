using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Service.Options
{
    public record JwtOptions
    {
        public const string SectionName = "Jwt";
        [Required] public required string Key { get; init; }
    }
}
