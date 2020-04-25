using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MatchBox.Data.Model
{
    // Passwords and how to store them right: https://crackstation.net/hashing-security.htm
    public class DbUser : DbEntityBase
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsDisabled { get; set; }

        public ICollection<DbCustomClaim> Attributes { get; set; } = new Collection<DbCustomClaim>();
        public ICollection<DbUserGroup> UserGroups { get; set; } = new Collection<DbUserGroup>();

        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}