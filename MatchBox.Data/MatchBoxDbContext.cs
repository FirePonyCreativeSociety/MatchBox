using MatchBox.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MatchBox.Data
{
    public class MatchBoxDbContext : IdentityDbContext<
        DbUser, DbRole, int,
        DbUserClaim, DbUserRole, DbUserLogin,
        DbRoleClaim, DbUserToken>
    {
        public const string DbConnectionName = "MatchBoxDb";
        public const string AdminUserName = "admin";
        const string SecuritySchemaName = null;// "Security";

        public MatchBoxDbContext(DbContextOptions<MatchBoxDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<DbGroup> Groups { get; set; }
        public DbSet<DbUserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DbUser>(b =>
            {
                b.ToTable(name: "Users", schema: SecuritySchemaName);
                b.Property(e => e.Id).HasColumnName("UserId");

                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                // Each User can have many UserGroups
                b.HasMany(e => e.UserGroups)
                 .WithOne(e => e.User)
                 .HasForeignKey(ur => ur.UserId)
                 .IsRequired();
            });

            builder.Entity<DbRole>(b =>
            {
                b.ToTable(name: "Roles", schema: SecuritySchemaName);
                b.Property(e => e.Id).HasColumnName("RoleId");

                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            builder.Entity<DbUserClaim>(entity =>
            {
                entity.ToTable("UserClaims", schema: SecuritySchemaName);
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.Id).HasColumnName("UserClaimId");

            });

            builder.Entity<DbUserLogin>(entity =>
            {
                entity.ToTable("UserLogins", schema: SecuritySchemaName);
                entity.Property(e => e.UserId).HasColumnName("UserId");

            });

            builder.Entity<DbRoleClaim>(entity =>
            {
                entity.ToTable("RoleClaims", schema: SecuritySchemaName);
                entity.Property(e => e.Id).HasColumnName("RoleClaimId");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");
            });

            builder.Entity<DbUserRole>(entity =>
            {
                entity.ToTable("UserRoles", schema: SecuritySchemaName);
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");

            });

            builder.Entity<DbUserToken>(entity =>
            {
                entity.ToTable("UserTokens", schema: SecuritySchemaName);
                entity.Property(e => e.UserId).HasColumnName("UserId");

            });

            builder.Entity<DbGroup>(entity =>
            {
                entity.ToTable("Groups", schema: SecuritySchemaName);

                // Each Group can have many entries in the UserGroupRole join table
                entity.HasMany(e => e.GroupUsers)
                    .WithOne(e => e.Group)
                    .HasForeignKey(ur => ur.GroupId)
                    .IsRequired();                
            });

            builder.Entity<DbUserGroup>(entity =>
            {
                entity.ToTable("UserGroups", schema: SecuritySchemaName);
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.GroupId).HasColumnName("GroupId");
            });

            builder.Entity<DbEvent>(entity =>
            {
                entity.ToTable("Events", schema: SecuritySchemaName);
                entity.HasKey(nameof(DbEvent.Id));
            });
        }
    }
}