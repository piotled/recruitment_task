using System.ComponentModel.DataAnnotations;

namespace RecruitmentTask.Api.DTO;

public class SubcategoryDTO
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
}
