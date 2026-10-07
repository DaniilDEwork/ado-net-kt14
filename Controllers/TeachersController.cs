using Kt14Education.Data;
using Kt14Education.Models;
using Kt14Education.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kt14Education.Controllers;

[Route("Teachers")]
public class TeachersController(AppDbContext db) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        return View(await db.Teachers.AsNoTracking().Include(teacher => teacher.Courses).ThenInclude(course => course.Students).OrderBy(teacher => teacher.TeacherId).ToListAsync());
    }

    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var teacher = await db.Teachers.AsNoTracking().Include(teacher => teacher.Courses).ThenInclude(course => course.Students).FirstOrDefaultAsync(teacher => teacher.TeacherId == id);
        return teacher is null ? NotFound() : View(teacher);
    }

    [HttpGet("Create")]
    public IActionResult Create() => View(new TeacherForm());

    [HttpPost("Create")]
    public async Task<IActionResult> Create(TeacherForm model)
    {
        if (!ModelState.IsValid) return View(model);
        var teacher = new Teacher { Name = model.Name.Trim() };
        db.Teachers.Add(teacher);
        await db.SaveChangesAsync();
        TempData["Success"] = "Преподаватель добавлен";
        return RedirectToAction(nameof(Details), new { id = teacher.TeacherId });
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var teacher = await db.Teachers.AsNoTracking().FirstOrDefaultAsync(teacher => teacher.TeacherId == id);
        return teacher is null ? NotFound() : View(new TeacherForm { TeacherId = id, Name = teacher.Name });
    }

    [HttpPost("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, TeacherForm model)
    {
        var teacher = await db.Teachers.FindAsync(id);
        if (teacher is null) return NotFound();
        model.TeacherId = id;
        if (!ModelState.IsValid) return View(model);
        teacher.Name = model.Name.Trim();
        await db.SaveChangesAsync();
        TempData["Success"] = "Данные преподавателя обновлены";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var teacher = await db.Teachers.AsNoTracking().Include(teacher => teacher.Courses).ThenInclude(course => course.Students).FirstOrDefaultAsync(teacher => teacher.TeacherId == id);
        return teacher is null ? NotFound() : View(teacher);
    }

    [HttpPost("Delete/{id:int}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var teacher = await db.Teachers.FindAsync(id);
        if (teacher is null) return NotFound();
        db.Teachers.Remove(teacher);
        await db.SaveChangesAsync();
        TempData["Success"] = "Преподаватель и все его курсы удалены";
        return RedirectToAction(nameof(Index));
    }
}
