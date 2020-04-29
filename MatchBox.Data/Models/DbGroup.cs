using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MatchBox.Data.Models
{
    public class DbGroup
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [Required]
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public virtual ICollection<DbUserGroup> GroupUsers { get; set; }        
    }
}
