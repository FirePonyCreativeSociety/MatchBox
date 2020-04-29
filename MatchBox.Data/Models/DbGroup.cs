using System;
using System.Collections.Generic;
using System.Text;

namespace MatchBox.Data.Models
{
    public class DbGroup
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual ICollection<DbUserGroup> GroupUsers { get; set; }        
    }
}
