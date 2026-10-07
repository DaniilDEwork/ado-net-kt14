using Kt14Education.Models;

namespace Kt14Education.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        if (!await db.Database.EnsureCreatedAsync()) return;
        var firstStudent = new Student { Name = "Даниил Денисенко" };
        var secondStudent = new Student { Name = "Анна Иванова" };
        db.Teachers.AddRange(
            new Teacher
            {
                Name = "Мария Смирнова",
                Courses = new List<Course>
                {
                    new() { Title = "ASP.NET Core", Students = new List<Student> { firstStudent, secondStudent } },
                    new() { Title = "ADO.NET", Students = new List<Student> { firstStudent } }
                }
            },
            new Teacher
            {
                Name = "Иван Петров",
                Courses = new List<Course>
                {
                    new() { Title = "Основы C#", Students = new List<Student> { secondStudent } }
                }
            });
        await db.SaveChangesAsync();
    }
}
