namespace RecruitmentTask.Frontend.Model;

/// <summary>
/// Klasa pozwalająca na wykonywanie operacji CRUD na użytkownikach
/// </summary>
public interface IUsersDAO
{
    /// <summary>
    /// Tworzy nowego użytkownika o podanym loginie i haśle
    /// </summary>
    /// <returns>True w przypadku pomyślnego utworzenia użytkownika, false w przeciwnym wypadku</returns>
    Task<bool> Create(string email, string password);
}
