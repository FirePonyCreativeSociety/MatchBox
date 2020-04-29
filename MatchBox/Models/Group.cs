using MatchBox.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MatchBox.API.Models
{
    public class Group : IIntId
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [Required]
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public ICollection<UserGroup> GroupUsers { get; set; } = new Collection<UserGroup>();

        int IIntId.GetId() => this.GroupId;
    }
}
