using System.Net.Http.Json;

namespace FrontendBlazor.Model;

public class UsersDAO
{
    private readonly HttpClient httpClient;

    public UsersDAO(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("RestApi");
    }

    public async Task<bool> Create(string email, string password)
    {
        var response = await httpClient.PostAsync("api/users/create", 
            JsonContent.Create(new { email, password }));
        return response.IsSuccessStatusCode;
    }
}
