using MatchBox.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace MatchBox.Data
{
    public class MatchBoxDbContext : IdentityDbContext<DbUser>
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
            modelBuilder.Entity<DbUserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });

            modelBuilder.Entity<DbUserGroup>()
                .HasOne(userGrp => userGrp.User)
                .WithMany(user => user.UserGroups)
                .HasForeignKey(userGrp => userGrp.GroupId);

            modelBuilder.Entity<DbUserGroup>()
                .HasOne(userGrp => userGrp.Group)
                .WithMany(grp => grp.UserGroups)
                .HasForeignKey(userGrp => userGrp.UserId);
        }

        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbCustomClaim> CustomClaims { get; set; }
        public DbSet<DbGroup> Groups { get; set; }
        public DbSet<DbUserGroup> UserGroups { get; set; }
        public DbSet<DbEvent> Events { get; set; }
    }
}