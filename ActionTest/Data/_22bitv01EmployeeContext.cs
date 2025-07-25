using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ActionTest.Models;

namespace ActionTest.Data;

public partial class _22bitv01EmployeeContext : DbContext
{
    public _22bitv01EmployeeContext()
    {
    }

    public _22bitv01EmployeeContext(DbContextOptions<_22bitv01EmployeeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-J6GM0VPI;Database=22BITV01_Employee;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BED2A14F05B");

            entity.ToTable("Department");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.DepartmentName).HasMaxLength(100);
            entity.Property(e => e.OfficePhone).HasMaxLength(20);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F11614AD8E2");

            entity.ToTable("Employee");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EmployeeName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PhotoImagePath).HasMaxLength(255);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__Depart__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

public DbSet<ActionTest.Models.Stat> Stat { get; set; } = default!;
}
