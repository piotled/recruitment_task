using System.ComponentModel.DataAnnotations;

namespace FrontendBlazor.Model;

public class Contact
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Surname { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public int CategoryId { get; set; }
    
    public int SubcategoryId { get; set; }
}
