namespace Task1.DataAccess;

public class Category
{
    public int Id { get; set; }
    public int Name { get; set; }
    public ICollection<Category> Subcategories { get; set; } = null!;
}
