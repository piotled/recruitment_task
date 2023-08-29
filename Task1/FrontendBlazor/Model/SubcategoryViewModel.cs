namespace RecruitmentTask.Frontend.Model;

/// <summary>
/// Model podkategorii po stronie klienta
/// </summary>
public class SubcategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}
