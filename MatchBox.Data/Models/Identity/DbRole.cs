using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatchBox.Data.Models
{
    public class DbRole : IdentityRole<int>
    {
        public string Description { get; set; }

        public virtual ICollection<DbUserRole> UserRoles { get; set; }
        public virtual ICollection<DbRoleClaim> RoleClaims { get; set; }
    }
}
