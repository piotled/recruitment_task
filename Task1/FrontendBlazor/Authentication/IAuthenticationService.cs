namespace RecruitmentTask.Frontend.Authentication;

/// <summary>
/// Służy do logowania/wylogowywania oraz sprawdzania czy użytkownik jest aktualnie zalogowany
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Uwierzytelnia użytkownika o podanym adresie email i haśle
    /// </summary>
    /// <returns>
    /// Zwraca true po poprawnym zalogowaniu. 
    /// Zwraca fałsz w przypadku błędu komunikacji z serwerem bądź nieprawidłowych danych logowania.
    /// </returns>
    public Task<bool> Authenticate(string email, string password);

    /// <summary>
    /// Sprawdza, czy użytkownik jest aktualnie zalogowany.
    /// </summary>
    /// <returns>
    /// Zwraca true gdy użytkownik jest zalgowany. False gdy nie jest zalogowany bądź nie można się połączyć z serwerem. 
    /// </returns>
    public Task<bool> IsAuthenticated();

    /// <summary>
    /// Wylogowuje użytkownika. 
    /// </summary>
    /// <returns>
    /// Zwraca true gdy użytkownik poprawnie się wylogował. False gdy nie można się połączyć z serwerem. 
    /// </returns>
    public Task<bool> Logout();
}