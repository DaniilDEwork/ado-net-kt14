using System.ComponentModel.DataAnnotations;

namespace Kt14Education.Models;

public class Teacher
{
    [Key]
    public int TeacherId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = "";

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
