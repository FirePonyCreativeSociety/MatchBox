using MatchBox.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MatchBox.Data
{
    public class MatchBoxDbContext : IdentityDbContext<DbUser, DbRole, int>
    {
        public const string DbConnectionName = "MatchBoxDb";
        public const string AdminUserName = "admin";
        const string SecuritySchemaName = "Security";

        public MatchBoxDbContext(DbContextOptions<MatchBoxDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);            

            builder.Entity<DbUser>(entity =>
            {
                entity.ToTable(name: "Users", schema: SecuritySchemaName);
                entity.Property(e => e.Id).HasColumnName("UserId");

                // Each User can have many UserClaims
                //entity.HasMany(e => e.Claims)
                //      .WithOne()
                //      .HasForeignKey(uc => uc.UserId)
                //      .IsRequired();
            });

            builder.Entity<DbRole>(entity =>
            {
                entity.ToTable(name: "Roles", schema: SecuritySchemaName);
                entity.Property(e => e.Id).HasColumnName("RoleId");

            });

            builder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaims", schema: SecuritySchemaName);
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.Id).HasColumnName("UserClaimId");

            });

            builder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogins", schema: SecuritySchemaName);
                entity.Property(e => e.UserId).HasColumnName("UserId");

            });

            builder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("RoleClaims", schema: SecuritySchemaName);
                entity.Property(e => e.Id).HasColumnName("RoleClaimId");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");
            });

            builder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("UserRoles", schema: SecuritySchemaName);
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");

            });

            builder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserTokens", schema: SecuritySchemaName);
                entity.Property(e => e.UserId).HasColumnName("UserId");

            });

            builder.Entity<DbEvent>(entity =>
            {
                entity.ToTable("Events", schema: SecuritySchemaName);
                entity.HasKey(nameof(DbEvent.Id));
            });
        }        
    }
}