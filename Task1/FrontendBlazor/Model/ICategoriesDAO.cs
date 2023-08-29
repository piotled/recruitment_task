namespace RecruitmentTask.Frontend.Model;

/// <summary>
/// Klasa pozwalająca na wykonywanie operacji CRUD na kategoriach
/// </summary>
public interface ICategoriesDAO
{
    /// <summary>
    /// Zwraca wszystkie kategorie wraz z ich podkategoriami
    /// </summary>
    Task<List<CategoryViewModel>> GetAll();
    
    /// <summary>
    /// Dodaje podkategorię kategorii "Inny"
    /// </summary>
    /// <param name="categoryName">Nazwa podkategorii do utworzenia</param>
    /// <returns>Identyfikator utworzonej kategorii. 0 w przypadku błędu.</returns>
    Task<int> AddOtherCategory(string categoryName);
}
