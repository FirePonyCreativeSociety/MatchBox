using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MatchBox.API.Model
{
    public class Group : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; } = new Collection<UserGroup>();
    }
}
