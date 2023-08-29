namespace RecruitmentTask.Frontend.Model;

/// <summary>
/// Model kategorii po stronie klienta
/// </summary>
public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<SubcategoryViewModel> Subcategories { get; set; } = new List<SubcategoryViewModel>();
}
