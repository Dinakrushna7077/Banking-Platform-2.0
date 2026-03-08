using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Banking_Platform_2._0.Models;

public partial class BankingDbContext : DbContext
{
    public BankingDbContext()
    {
    }

    public BankingDbContext(DbContextOptions<BankingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountMst> AccountMsts { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DINAKRUSHNA-200;Database=Banking_DB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountMst>(entity =>
        {
            entity.HasKey(e => e.AccId).HasName("PK__Account___91CBC378325A996C");

            entity.Property(e => e.Balance).HasDefaultValue(0m);
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.Branch).WithMany(p => p.AccountMsts).HasConstraintName("FK__Account_m__Branc__4BAC3F29");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__719FE4E8102867A2");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.Branch).WithMany(p => p.Admins)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Admin__BranchId__59FA5E80");

            entity.HasOne(d => d.User).WithMany(p => p.Admins).HasConstraintName("FK__Admin__UserId__571DF1D5");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__AuditLog__5E5486489558EBBB");

            entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuditLogs__UserI__412EB0B6");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__Branch__A1682FC54DDBAF52");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8D97A9DAC");

            entity.HasOne(d => d.Acc).WithMany(p => p.Customers)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Customer__AccId__5DCAEF64");

            entity.HasOne(d => d.User).WithMany(p => p.Customers).HasConstraintName("FK__Customer__UserId__5441852A");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1AE6B02CDB");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6B89020C43");

            entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.FromAc).WithMany(p => p.TransactionFromAcs).HasConstraintName("FK__Transacti__FromA__4E88ABD4");

            entity.HasOne(d => d.ToAc).WithMany(p => p.TransactionToAcs).HasConstraintName("FK__Transacti__ToAcI__4F7CD00D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CFD887DC8");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.Acc).WithMany(p => p.Users).HasConstraintName("fk_accid");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleId__3B75D760");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
