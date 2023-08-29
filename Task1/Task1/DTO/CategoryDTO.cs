using System.ComponentModel.DataAnnotations;

namespace RecruitmentTask.Api.DTO;

public class CategoryDTO
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
}
