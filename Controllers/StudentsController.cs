using Kt14Education.Data;
using Kt14Education.Models;
using Kt14Education.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kt14Education.Controllers;

[Route("Students")]
public class StudentsController(AppDbContext db) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        return View(await db.Students.AsNoTracking().Include(student => student.Courses).ThenInclude(course => course.Teacher).OrderBy(student => student.StudentId).ToListAsync());
    }

    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var student = await db.Students.AsNoTracking().Include(student => student.Courses).ThenInclude(course => course.Teacher).FirstOrDefaultAsync(student => student.StudentId == id);
        return student is null ? NotFound() : View(student);
    }

    [HttpGet("Create")]
    public IActionResult Create() => View(new StudentForm());

    [HttpPost("Create")]
    public async Task<IActionResult> Create(StudentForm model)
    {
        if (!ModelState.IsValid) return View(model);
        var student = new Student { Name = model.Name.Trim() };
        db.Students.Add(student);
        await db.SaveChangesAsync();
        TempData["Success"] = "Студент добавлен";
        return RedirectToAction(nameof(Details), new { id = student.StudentId });
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var student = await db.Students.AsNoTracking().FirstOrDefaultAsync(student => student.StudentId == id);
        return student is null ? NotFound() : View(new StudentForm { StudentId = id, Name = student.Name });
    }

    [HttpPost("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, StudentForm model)
    {
        var student = await db.Students.FindAsync(id);
        if (student is null) return NotFound();
        model.StudentId = id;
        if (!ModelState.IsValid) return View(model);
        student.Name = model.Name.Trim();
        await db.SaveChangesAsync();
        TempData["Success"] = "Данные студента обновлены";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await db.Students.AsNoTracking().Include(student => student.Courses).ThenInclude(course => course.Teacher).FirstOrDefaultAsync(student => student.StudentId == id);
        return student is null ? NotFound() : View(student);
    }

    [HttpPost("Delete/{id:int}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await db.Students.FindAsync(id);
        if (student is null) return NotFound();
        db.Students.Remove(student);
        await db.SaveChangesAsync();
        TempData["Success"] = "Студент и его записи на курсы удалены";
        return RedirectToAction(nameof(Index));
    }
}
