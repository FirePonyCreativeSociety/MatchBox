using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MatchBox.Data.Models
{
    public class DbCustomClaim : DbEntityBase
    {
        public DbUser User { get; set; }

        [Required]
        public string Name { get; set; }
        public string Value { get; set; }
    }
}