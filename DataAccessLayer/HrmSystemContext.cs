using System;
using System.Collections.Generic;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer;

public partial class HrmSystemContext : DbContext
{
    public HrmSystemContext()
    {
    }

    public HrmSystemContext(DbContextOptions<HrmSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<BackupHistory> BackupHistories { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<SessionHistory> SessionHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }
    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // Đảm bảo đường dẫn đúng
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        return configuration.GetConnectionString("DefaultConnection")
               ?? throw new InvalidOperationException("Database connection string is missing.");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.ActivityLogId).HasName("PK__Activity__19A9B7AFC663DAFD");

            entity.ToTable("ActivityLog");

            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.Details).HasMaxLength(255);
            entity.Property(e => e.TableName).HasMaxLength(50);
            entity.Property(e => e.TablePrimaryKey).HasMaxLength(50);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ActivityL__UserI__5DCAEF64");
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Attendan__8B69261CA054230C");

            entity.ToTable("Attendance");

            entity.Property(e => e.LeaveType).HasMaxLength(50);
            entity.Property(e => e.OvertimeHours).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.WorkDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Attendanc__Emplo__52593CB8");
        });

        modelBuilder.Entity<BackupHistory>(entity =>
        {
            entity.HasKey(e => e.BackupId).HasName("PK__BackupHi__EB9069C22F26C7E1");

            entity.ToTable("BackupHistory");

            entity.Property(e => e.BackupDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.BackupFilePath).HasMaxLength(255);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BED96123AFE");

            entity.HasIndex(e => e.DepartmentName, "UQ__Departme__D949CC34494ED833").IsUnique();

            entity.Property(e => e.DepartmentName).HasMaxLength(100);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F116606D5B7");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Employee__85FB4E38D601C7F3").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Employee__A9D10534C8AF6CE0").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Position).HasMaxLength(50);
            entity.Property(e => e.ProfileImage).HasMaxLength(255);
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employees__Depar__3F466844");
        });

        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.HasKey(e => e.LeaveId).HasName("PK__LeaveReq__796DB95935F845F6");

            entity.Property(e => e.LeaveType).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Ch? duy?t");

            entity.HasOne(d => d.Employee).WithMany(p => p.LeaveRequests)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__LeaveRequ__Emplo__6383C8BA");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E12D5837D2F");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Message).HasMaxLength(1000);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.TargetDepartment).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.TargetDepartmentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Notificat__Targe__5629CD9C");
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasKey(e => e.SalaryId).HasName("PK__Salaries__4BE20457D9B4E2F6");

            entity.Property(e => e.Allowance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BasicSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Bonus).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Deduction).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PayDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.Salaries)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Salaries__Employ__4CA06362");
        });

        modelBuilder.Entity<SessionHistory>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__SessionH__C9F492905F937A1D");

            entity.ToTable("SessionHistory");

            entity.Property(e => e.DeviceInfo).HasMaxLength(255);
            entity.Property(e => e.LoginTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LogoutTime).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.SessionHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SessionHi__UserI__59FA5E80");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C8660885D");

            entity.HasIndex(e => e.EmployeeId, "UQ__Users__7AD04F1023968539").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ__Users__85FB4E38F4F16D98").IsUnique();

            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Role).HasMaxLength(20);

            entity.HasOne(d => d.Employee).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Users__EmployeeI__44FF419A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
