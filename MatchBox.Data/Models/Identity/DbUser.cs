﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MatchBox.Data.Models
{
    // Passwords and how to store them right: https://crackstation.net/hashing-security.htm
    public class DbUser : IdentityUser<int>
    {
        [Required]
        public bool IsDisabled { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required] 
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string Avatar { get; set; }

        public virtual ICollection<DbUserClaim> Claims { get; set; }
        public virtual ICollection<DbUserLogin> Logins { get; set; }
        public virtual ICollection<DbUserToken> Tokens { get; set; }
        public virtual ICollection<DbUserRole> UserRoles { get; set; }
        public virtual ICollection<DbUserGroup> UserGroups { get; set; }
    }
}