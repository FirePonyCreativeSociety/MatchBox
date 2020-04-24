using MatchBox.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Db
{
    public class MatchBoxDbContext : DbContext
    {
        public const string DbConnectionName = "MatchBoxDb";
        public const string AdminUserName = "admin";

        public MatchBoxDbContext(DbContextOptions<MatchBoxDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });

            modelBuilder.Entity<UserGroup>()
                .HasOne(userGrp => userGrp.User)
                .WithMany(user => user.UserGroups)
                .HasForeignKey(userGrp => userGrp.GroupId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(userGrp => userGrp.Group)
                .WithMany(grp => grp.UserGroups)
                .HasForeignKey(userGrp => userGrp.UserId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CustomClaim> CustomClaims { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}
