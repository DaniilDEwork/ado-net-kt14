using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kt14Education.Models;

public class Course
{
    [Key]
    public int CourseId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = "";

    [ForeignKey(nameof(Teacher))]
    public int TeacherId { get; set; }

    public Teacher Teacher { get; set; } = null!;
    public ICollection<Student> Students { get; set; } = new List<Student>();
}
