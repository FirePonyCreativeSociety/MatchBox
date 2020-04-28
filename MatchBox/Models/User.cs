using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MatchBox.API.Models
{
    public class User : EntityBase
    {        
        public string UserName { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsDisabled { get; set; }

        public ICollection<CustomClaim> Attributes { get; set; } = new Collection<CustomClaim>();
        public ICollection<UserGroup> UserGroups { get; set; } = new Collection<UserGroup>();
    }
}