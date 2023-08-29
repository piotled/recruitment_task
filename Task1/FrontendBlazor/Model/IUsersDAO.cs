namespace FrontendBlazor.Model;

public interface IUsersDAO
{
    Task<bool> Create(string email, string password);
}
