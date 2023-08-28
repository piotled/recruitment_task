namespace Task1.Authorization;

public interface ITokenManager
{
    Task CancelToken(string tokenId);
    Task<string> CreateToken(string userId);
}
