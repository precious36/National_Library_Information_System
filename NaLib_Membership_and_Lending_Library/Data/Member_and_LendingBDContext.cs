using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NaLib_Membership_and_Lending_Lib.Data;

public partial class Member_and_LendingBDContext : DbContext
{
    public Member_and_LendingBDContext()
    {
    }

    public Member_and_LendingBDContext(DbContextOptions<Member_and_LendingBDContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DamageAndLoss> DamageAndLosses { get; set; }

    public virtual DbSet<LendingPreference> LendingPreferences { get; set; }

    public virtual DbSet<LendingTransaction> LendingTransactions { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DamageAndLoss>(entity =>
        {
            entity.HasKey(e => e.DamageId).HasName("PK__damage_a__A7D67E8D38C2A77E");

            entity.HasOne(d => d.Transaction).WithMany(p => p.DamageAndLosses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_damage_and_loss_lending_transactions");
        });

        modelBuilder.Entity<LendingPreference>(entity =>
        {
            entity.HasKey(e => e.PreferenceId).HasName("PK__lending___FB41DBCFD1142B12");

            entity.HasOne(d => d.Member).WithMany(p => p.LendingPreferences)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_lending_preferences_members");
        });

        modelBuilder.Entity<LendingTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__lending___85C600AFB46A3BD0");

            entity.Property(e => e.Status).HasDefaultValue("checked_out");

            entity.HasOne(d => d.Member).WithMany(p => p.LendingTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_lending_transactions_members");

            entity.HasOne(d => d.Resource).WithMany(p => p.LendingTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_lending_transactions_resources");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__members__B29B853499A0A346");

            entity.Property(e => e.Status).HasDefaultValue("active");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.ResourceId).HasName("PK__resource__4985FC735180CD93");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
