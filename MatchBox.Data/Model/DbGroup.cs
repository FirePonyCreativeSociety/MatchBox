using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MatchBox.Data.Model
{
    public class DbGroup : DbEntityBase
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<DbUserGroup> UserGroups { get; set; } = new Collection<DbUserGroup>();
    }
}
