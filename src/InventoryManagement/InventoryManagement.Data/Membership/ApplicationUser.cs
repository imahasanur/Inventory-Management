using InventoryManagement.Data.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Data.Membership
{
    public class ApplicationUser : IdentityUser<Guid>, ITimeStamp
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
		public DateTime CreatedAtUtc { get ; set; }
		public DateTime? UpdatedAtUtc { get ; set ; }
	}
}
