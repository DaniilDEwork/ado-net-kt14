using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kt14Education.ViewModels;

public class TeacherForm
{
    public int TeacherId { get; set; }

    [Required(ErrorMessage = "Введите имя преподавателя")]
    [StringLength(100, ErrorMessage = "Имя не должно быть длиннее 100 символов")]
    public string Name { get; set; } = "";
}

public class StudentForm
{
    public int StudentId { get; set; }

    [Required(ErrorMessage = "Введите имя студента")]
    [StringLength(100, ErrorMessage = "Имя не должно быть длиннее 100 символов")]
    public string Name { get; set; } = "";
}

public class CourseForm
{
    public int CourseId { get; set; }

    [Required(ErrorMessage = "Введите название курса")]
    [StringLength(200, ErrorMessage = "Название не должно быть длиннее 200 символов")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Выберите преподавателя")]
    public int? TeacherId { get; set; }

    public List<int> StudentIds { get; set; } = new();

    [ValidateNever]
    public List<SelectListItem> Teachers { get; set; } = new();

    [ValidateNever]
    public List<StudentChoice> Students { get; set; } = new();
}

public class StudentChoice
{
    public int StudentId { get; set; }
    public string Name { get; set; } = "";
}

public class HomeModel
{
    public int TeachersCount { get; set; }
    public int StudentsCount { get; set; }
    public int CoursesCount { get; set; }
    public int EnrollmentsCount { get; set; }
}
