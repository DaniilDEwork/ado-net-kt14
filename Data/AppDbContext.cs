using Kt14Education.Models;
using Microsoft.EntityFrameworkCore;

namespace Kt14Education.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<StudentCourse> StudentCourses => Set<StudentCourse>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Teacher>()
            .HasMany(teacher => teacher.Courses)
            .WithOne(course => course.Teacher)
            .HasForeignKey(course => course.TeacherId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Student>()
            .HasMany(student => student.Courses)
            .WithMany(course => course.Students)
            .UsingEntity<StudentCourse>(
                link => link.HasOne(item => item.Course).WithMany()
                    .HasForeignKey(item => item.CourseId).OnDelete(DeleteBehavior.Cascade),
                link => link.HasOne(item => item.Student).WithMany()
                    .HasForeignKey(item => item.StudentId).OnDelete(DeleteBehavior.Cascade),
                link =>
                {
                    link.HasKey(item => new { item.StudentId, item.CourseId });
                    link.ToTable("StudentCourses");
                });
    }
}
