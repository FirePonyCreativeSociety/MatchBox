using MatchBox.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MatchBox.API.Models
{
    public class Group : IIntId
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; } = new Collection<UserGroup>();

        int IIntId.GetId() => this.GroupId;
    }
}
