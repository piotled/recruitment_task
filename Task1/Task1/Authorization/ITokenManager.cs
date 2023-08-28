namespace RecruitmentTask.Api.Authorization;

/// <summary>
/// Interfejs dla klas zajmujących się tworzeniem i unieważnianiem tokenów JWT
/// </summary>
public interface ITokenManager
{
    /// <summary>
    /// Tworzy token dla użytkownika o podanym <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">Id użytkownika w bazie. Użyte jako pole "sub" tokenu.</param>
    /// <returns>
    /// Utworzony token JWT
    /// </returns>
    Task<string> CreateToken(string userId);
    
    /// <summary>
    /// Unieważnia token o podanym identyfikatorze, <see cref="TokenNotCancelledAuthorizationHandler">
    /// </summary>
    /// <param name="tokenId">Identyfikator tokenu w bazie, pobrany z pola "TokenId" utworzonego tokenu</param>
    Task CancelToken(string tokenId);
}
