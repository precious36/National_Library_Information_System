using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NaLib_Staff_Management_Lib.Data;

public partial class NaLib_context : DbContext
{
    public NaLib_context()
    {
    }

    public NaLib_context(DbContextOptions<NaLib_context> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<FailedLoginAttempt> FailedLoginAttempts { get; set; }

    public virtual DbSet<Library> Libraries { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<PasswordHistory> PasswordHistories { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermmsion> RolePermmsions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.DepartmentId).ValueGeneratedNever();
        });

        modelBuilder.Entity<FailedLoginAttempt>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.FailedLoginAttempts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Failed_Login_Attempts_Users");
        });

        modelBuilder.Entity<Library>(entity =>
        {
            entity.Property(e => e.LibraryId).ValueGeneratedNever();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.Property(e => e.LocationId).ValueGeneratedNever();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<PasswordHistory>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.PasswordHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PasswordHistory_Users");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.Property(e => e.PermissionId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<RolePermmsion>(entity =>
        {
            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermmsions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermmsions_Permissions");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.UserDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserDetails_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
