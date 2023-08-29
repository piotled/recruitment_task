using System.ComponentModel.DataAnnotations;

namespace RecruitmentTask.Api.DTO;

public class UserDTO
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}
