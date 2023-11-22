using Microsoft.EntityFrameworkCore;
using Ramboe.IS4.Data.Models;

namespace Ramboe.IS4.Data;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
        //Prevent DateTime error
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure UserEntity
        modelBuilder.Entity<UserEntity>()
                    .HasKey(u => u.Id);// Set Id as the primary key for UserEntity

        modelBuilder.Entity<UserEntity>()
                    .Property(u => u.Id)
                    .ValueGeneratedOnAdd();// Automatically generate Id on insert

        modelBuilder.Entity<UserEntity>()
                    .HasOne(u => u.Role)// Each user has one role
                    .WithMany(r => r.Users)// Each role can have multiple users
                    .IsRequired();// Role is required for a user

        // Configure RoleEntity
        modelBuilder.Entity<RoleEntity>()
                    .HasKey(r => r.Id);// Set Id as the primary key for RoleEntity

        modelBuilder.Entity<RoleEntity>()
                    .Property(r => r.Id)
                    .ValueGeneratedOnAdd();// Automatically generate Id on insert

        modelBuilder.Entity<RoleEntity>()
                    .HasMany(r => r.Users)// Each role can have multiple users
                    .WithOne(u => u.Role);// Each user has one role
    }
}