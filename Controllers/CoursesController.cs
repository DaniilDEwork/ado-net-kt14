using Kt14Education.Data;
using Kt14Education.Models;
using Kt14Education.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kt14Education.Controllers;

[Route("Courses")]
public class CoursesController(AppDbContext db) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        return View(await db.Courses.AsNoTracking().Include(course => course.Teacher)
            .Include(course => course.Students).OrderBy(course => course.CourseId).ToListAsync());
    }

    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var course = await db.Courses.AsNoTracking().Include(course => course.Teacher)
            .Include(course => course.Students).FirstOrDefaultAsync(course => course.CourseId == id);
        return course is null ? NotFound() : View(course);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create(int? teacherId)
    {
        var model = new CourseForm { TeacherId = teacherId };
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CourseForm model)
    {
        await ValidateRelationsAsync(model);
        if (!ModelState.IsValid)
        {
            await LoadOptionsAsync(model);
            return View(model);
        }
        var course = new Course
        {
            Title = model.Title.Trim(),
            TeacherId = model.TeacherId!.Value,
            Students = await db.Students.Where(student => model.StudentIds.Contains(student.StudentId)).ToListAsync()
        };
        db.Courses.Add(course);
        await db.SaveChangesAsync();
        TempData["Success"] = "Курс добавлен";
        return RedirectToAction(nameof(Details), new { id = course.CourseId });
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var course = await db.Courses.AsNoTracking().Include(course => course.Students)
            .FirstOrDefaultAsync(course => course.CourseId == id);
        if (course is null) return NotFound();
        var model = new CourseForm
        {
            CourseId = id, Title = course.Title, TeacherId = course.TeacherId,
            StudentIds = course.Students.Select(student => student.StudentId).ToList()
        };
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, CourseForm model)
    {
        var course = await db.Courses.Include(course => course.Students)
            .FirstOrDefaultAsync(course => course.CourseId == id);
        if (course is null) return NotFound();
        model.CourseId = id;
        await ValidateRelationsAsync(model);
        if (!ModelState.IsValid)
        {
            await LoadOptionsAsync(model);
            return View(model);
        }
        course.Title = model.Title.Trim();
        course.TeacherId = model.TeacherId!.Value;
        course.Students = await db.Students.Where(student => model.StudentIds.Contains(student.StudentId)).ToListAsync();
        await db.SaveChangesAsync();
        TempData["Success"] = "Курс и записи студентов обновлены";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var course = await db.Courses.AsNoTracking().Include(course => course.Teacher)
            .Include(course => course.Students).FirstOrDefaultAsync(course => course.CourseId == id);
        return course is null ? NotFound() : View(course);
    }

    [HttpPost("Delete/{id:int}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await db.Courses.FindAsync(id);
        if (course is null) return NotFound();
        db.Courses.Remove(course);
        await db.SaveChangesAsync();
        TempData["Success"] = "Курс и записи на него удалены";
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidateRelationsAsync(CourseForm model)
    {
        if (model.TeacherId.HasValue && !await db.Teachers.AnyAsync(teacher => teacher.TeacherId == model.TeacherId.Value))
        {
            ModelState.AddModelError(nameof(CourseForm.TeacherId), "Выбранный преподаватель не найден");
        }
        var ids = model.StudentIds.Distinct().ToList();
        if (await db.Students.CountAsync(student => ids.Contains(student.StudentId)) != ids.Count)
        {
            ModelState.AddModelError(nameof(CourseForm.StudentIds), "Один из выбранных студентов не найден");
        }
    }

    private async Task LoadOptionsAsync(CourseForm model)
    {
        model.Teachers = await db.Teachers.AsNoTracking().OrderBy(teacher => teacher.Name)
            .Select(teacher => new SelectListItem { Value = teacher.TeacherId.ToString(), Text = teacher.Name }).ToListAsync();
        model.Students = await db.Students.AsNoTracking().OrderBy(student => student.Name)
            .Select(student => new StudentChoice { StudentId = student.StudentId, Name = student.Name }).ToListAsync();
    }
}
