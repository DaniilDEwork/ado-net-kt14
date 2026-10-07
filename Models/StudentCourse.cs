using System.ComponentModel.DataAnnotations.Schema;

namespace Kt14Education.Models;

public class StudentCourse
{
    [ForeignKey(nameof(Student))]
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}
