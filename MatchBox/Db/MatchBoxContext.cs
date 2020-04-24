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
        public const string DbConnectionName = "MatchBoxDb";
        public const string AdminUserName = "admin";

        public MatchBoxContext(DbContextOptions<MatchBoxContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ThemeCamp> ThemeCamps { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }
    }
}
