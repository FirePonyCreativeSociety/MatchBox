using System;
using System.Collections.Generic;

namespace MatchBox.Contracts
{
    public class User : EntityBase
    {        
        public string UserName { get; set; }
        //public string DisplayName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsDisabled { get; set; }
        public ICollection<UserAttribute> Attributes { get; set; }
    }
}