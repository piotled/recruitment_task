namespace RecruitmentTask.Frontend.Model;

/// <summary>
/// Klasa pozwalająca na wykonywanie operacji CRUD na kontaktach
/// </summary>
public interface IContactsDAO
{
    /// <summary>
    /// Zwraca wszystkie kontakty
    /// </summary>
    Task<List<ContactViewModel>> GetAll();
    
    /// <summary>
    /// Zwraca kontakt o podanym id bądź null w przypadku błędu
    /// </summary>
    Task<ContactViewModel?> Get(int id);
    
    /// <summary>
    /// Tworzy nowy kontakt dla podanych danych
    /// </summary>
    /// <returns>True gdy pomyślnie dodano kontakt, false w przypadku błędu</returns>
    Task<bool> Create(ContactViewModel contact);

    /// <summary>
    /// Aktualizuje podany kontakt
    /// </summary>
    /// <param name="contact">
    /// Dane kontaktu. 
    /// Muszą zawierać identyfikator istniejącego kontaktu.
    /// </param>
    /// <returns>True gdy pomyślnie zaktualizowano kontakt, false w przypadku błędu</returns>
    Task<bool> Update(ContactViewModel contact);

    /// <summary>
    /// Usuwa wskazany kontakt wraz z jego dodatkową podkategorią 
    /// dla kategorii "Inny" (jeżel taka istnieje).
    /// </summary>
    /// <returns>True gdy pomyślnie usunięto kontakt, false w przypadku błędu</returns>
    Task<bool> Delete(int contactId);
}