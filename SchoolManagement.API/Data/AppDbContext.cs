using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Models.Entities;

namespace SchoolManagement.API.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Admin> Admins { get; private set; } = default!;
    public DbSet<Attendance> Attendances { get; private set; } = default!;
    public DbSet<Class> Classes { get; private set; } = default!;
    public DbSet<ClassSubject> ClassSubjects { get; private set; } = default!;
    public DbSet<Grade> Grades { get; private set; } = default!;
    public DbSet<Student> Students { get; private set; } = default!;
    public DbSet<Teacher> Teachers { get; private set; } = default!;
    public DbSet<Schedule> Schedules { get; private set; } = default!;
    public DbSet<Subject> Subjects { get; private set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Admin (1) → (1) User (mandatory)
        modelBuilder.Entity<Admin>()
            .HasOne(a => a.User)
            .WithOne(u => u.Admin)
            .HasForeignKey<Admin>(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Student → Class (many-to-one)
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Class)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.ClassId)
            .OnDelete(DeleteBehavior.Restrict);

        // Student (1) → (1) User (mandatory)
        modelBuilder.Entity<Student>()
            .HasOne(s => s.User)
            .WithOne(u => u.Student)
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Teacher (1) → (1) User (mandatory)
        modelBuilder.Entity<Teacher>()
            .HasOne(t => t.User)
            .WithOne(u => u.Teacher)
            .HasForeignKey<Teacher>(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Class → HomeroomTeacher (optional one-to-many)
        modelBuilder.Entity<Class>()
            .HasOne(c => c.HomeroomTeacher)
            .WithMany(t => t.HomeroomClasses)
            .HasForeignKey(c => c.HomeroomTeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ClassSubject>()
            .HasOne(cs => cs.Class)
            .WithMany(c => c.ClassSubjects)
            .HasForeignKey(cs => cs.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClassSubject>()
            .HasOne(cs => cs.Subject)
            .WithMany(s => s.ClassSubjects)
            .HasForeignKey(cs => cs.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClassSubject>()
            .HasOne(cs => cs.Teacher)
            .WithMany(t => t.ClassSubjects)
            .HasForeignKey(cs => cs.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        // Schedule → Class, Subject, Teacher
        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Class)
            .WithMany(c => c.Schedules)
            .HasForeignKey(s => s.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Subject)
            .WithMany(sub => sub.Schedules)
            .HasForeignKey(s => s.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Teacher)
            .WithMany(t => t.Lessons)
            .HasForeignKey(s => s.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        // Attendance → Student, Schedule
        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Student)
            .WithMany(s => s.Attendances)
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Schedule)
            .WithMany(s => s.Attendances)
            .HasForeignKey(a => a.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}