using System;
using System.Collections.Generic;
using System.Text;

namespace MatchBox.Data.Model
{
    public class DbCustomClaim : DbEntityBase
    {
        public DbUser User { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}