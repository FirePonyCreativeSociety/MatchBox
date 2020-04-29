using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatchBox.Data.Models
{
    public class DbUserClaim : IdentityUserClaim<int>
    {
        public virtual DbUser User { get; set; }
    }
}
