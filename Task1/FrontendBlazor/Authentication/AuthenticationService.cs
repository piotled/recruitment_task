using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace FrontendBlazor.Authentication;

public class AuthenticationService
{
    private readonly HttpClient httpClient;
    private readonly ITokenStorage tokenStorage;

    public AuthenticationService(IHttpClientFactory httpClientFactory, ITokenStorage tokenStorage)
    {
        httpClient = httpClientFactory.CreateClient("RestApi");
        this.tokenStorage = tokenStorage;
    }

    public async Task<bool> Authenticate(string email, string password)
    {
        var response = await httpClient.PostAsync("api/authentication/accesstoken", 
            JsonContent.Create(new { email, password }));
        
        if (response.IsSuccessStatusCode)
        {
            string currentToken = await response.Content.ReadAsStringAsync();
            await tokenStorage.StoreToken(currentToken);
            return true;
        }
        else
            return false;
    }

    public async Task<bool> IsAuthenticated()
    {
        string currentToken = await tokenStorage.GetToken();
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", currentToken);
        var response = await httpClient.GetAsync("api/authentication/status");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Logout()
    {
        string currentToken = await tokenStorage.GetToken();
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", currentToken);
        var response = await httpClient.PostAsync("api/authentication/logout", null);
        return response.IsSuccessStatusCode;
    }
}
