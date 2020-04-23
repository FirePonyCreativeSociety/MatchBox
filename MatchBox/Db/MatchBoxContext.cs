using MatchBox.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Db
{
    public class MatchBoxContext : DbContext
    {
        public MatchBoxContext(DbContextOptions<MatchBoxContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<ThemeCamp> ThemeCamps { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }
    }
}
