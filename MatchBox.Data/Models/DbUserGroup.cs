using System;
using System.Collections.Generic;
using System.Text;

namespace MatchBox.Data.Models
{
    public class DbUserGroup
    {
        public long UserId { get; set; }
        public DbUser User { get; set; }

        public long GroupId { get; set; }
        public DbGroup Group { get; set; }
    }
}