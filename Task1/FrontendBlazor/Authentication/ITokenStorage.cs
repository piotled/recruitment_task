namespace FrontendBlazor.Authentication;

/// <summary>
/// Klasa odpowiedzialna za przechowywanie tokenu JWT użytego w procesie uwierzytelniania
/// </summary>
public interface ITokenStorage
{
    /// <summary>
    /// Pobiera zachowany token.
    /// </summary>
    Task<string> GetToken();
    
    /// <summary>
    /// Zapisuje wskazany token
    /// </summary>
    Task StoreToken(string token);
}
