namespace FrontendBlazor.Authentication;

public interface ITokenStorage
{
    Task<string> GetToken();
    Task StoreToken(string token);
}
