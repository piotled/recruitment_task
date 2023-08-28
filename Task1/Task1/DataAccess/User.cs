using Microsoft.AspNetCore.Identity;

namespace Task1.DataAccess;

public class User : IdentityUser<int>
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public ICollection<Contact> Contacts { get; set; } = null!;
}
