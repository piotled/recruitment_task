namespace RecruitmentTask.Api.DataAccess;

/// <summary>
/// Podkategoria dla kategorii.
/// W przypadku podkategorii należących do kategorii "Inny"
/// przechowuje nazwy specjalnych kategorii utworzonych dla konkretnych kontaktów.
/// </summary>
public class Subcategory
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
}
