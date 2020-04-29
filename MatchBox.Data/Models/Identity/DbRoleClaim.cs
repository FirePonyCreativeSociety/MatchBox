using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatchBox.Data.Models
{
    public class DbRoleClaim : IdentityRoleClaim<int>
    {
        public virtual DbRole Role { get; set; }
    }
}
