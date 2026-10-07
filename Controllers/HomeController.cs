using Kt14Education.Data;
using Kt14Education.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kt14Education.Controllers;

public class HomeController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(new HomeModel
        {
            TeachersCount = await db.Teachers.CountAsync(),
            StudentsCount = await db.Students.CountAsync(),
            CoursesCount = await db.Courses.CountAsync(),
            EnrollmentsCount = await db.StudentCourses.CountAsync()
        });
    }

    public IActionResult Error()
    {
        Response.StatusCode = 500;
        return View();
    }
}
