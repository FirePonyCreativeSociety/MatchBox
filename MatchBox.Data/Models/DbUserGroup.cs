using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MatchBox.Data.Models
{
    public class DbUserGroup
    {
        [Key]
        public int UserId { get; set; }
        public DbUser User { get; set; }

        public int GroupId { get; set; }
        public DbGroup Group { get; set; }
    }
}
