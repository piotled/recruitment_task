using FrontendBlazor.Validation;
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

    private string? email = null;

    [EmailAddress]
    public string? Email { get => email; set => email = value == string.Empty ? null : value; }

    [BirthDate(ErrorMessage = "Data urodzenia musi być później niż 1900-01-01")]
    public DateTime DateOfBirth { get; set; } = DateTime.Now.Date;

    public int CategoryId { get; set; }
    
    public int SubcategoryId { get; set; }
}
