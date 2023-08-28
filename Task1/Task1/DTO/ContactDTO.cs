using System.ComponentModel.DataAnnotations;

namespace RecruitmentTask.Api.DTO;

public class ContactDTO
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Surname { get; set; } = null!;

    [EmailAddress]
    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public int CategoryId { get; set; }

    public int SubcategoryId { get; set; }
}
