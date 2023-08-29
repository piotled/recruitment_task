using System.Net.Http.Json;

namespace RecruitmentTask.Frontend.Model.Impl;

public class UsersDAO : IUsersDAO
{
    private readonly HttpClient httpClient;

    public UsersDAO(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient("RestApi");
    }

    public async Task<bool> Create(string email, string password)
    {
        try
        {
            var response = await httpClient.PostAsync("api/users/create",
                JsonContent.Create(new { email, password }));
            return response.IsSuccessStatusCode;
        }
        catch
        {
            Console.WriteLine("Error in communication with server");
            return false;
        }
    }
}
