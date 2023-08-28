using System.ComponentModel.DataAnnotations.Schema;

namespace Task1.DataAccess;

public class Contact
{
    public int Id { get; set; }
    public int ReferencedUserId { get; set; }
    public User ReferencedUser { get; set; } = null!;
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public int SubcategoryId { get; set; }
    public Subcategory Subcategory { get; set; } = null!;
}
