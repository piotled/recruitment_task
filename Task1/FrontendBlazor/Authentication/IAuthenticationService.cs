namespace FrontendBlazor.Authentication;

public interface IAuthenticationService
{
    public Task<bool> Authenticate(string email, string password);
    public Task<bool> IsAuthenticated();
    public Task<bool> Logout();
}