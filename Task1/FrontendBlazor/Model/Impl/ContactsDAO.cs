using RecruitmentTask.Frontend.Authentication;
using System.Net.Http.Json;

namespace RecruitmentTask.Frontend.Model.Impl;

public class ContactsDAO : IContactsDAO
{
    private readonly HttpClient httpClient;
    private readonly ITokenStorage tokenStorage;

    public ContactsDAO(IHttpClientFactory httpClientFactory, ITokenStorage tokenStorage)
    {
        httpClient = httpClientFactory.CreateClient("RestApi");
        this.tokenStorage = tokenStorage;
    }

    public async Task<List<ContactViewModel>> GetAll()
    {
        try
        {
            return await httpClient.GetFromJsonAsync<List<ContactViewModel>>("api/contacts") ?? new();
        }
        catch
        {
            Console.WriteLine("Error in communication with server");
            return new();
        }
    }

    public async Task<ContactViewModel?> Get(int id)
    {
        try
        {
            string currentToken = await tokenStorage.GetToken();
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", currentToken);
            return await httpClient.GetFromJsonAsync<ContactViewModel>($"api/contacts/{id}");
        }
        catch
        {
            Console.WriteLine("Error in communication with server");
            return null;
        }
    }

    public async Task<bool> Create(ContactViewModel contact)
    {
        string currentToken = await tokenStorage.GetToken();
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", currentToken);
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/contacts", contact);
            return response.IsSuccessStatusCode;
        }
        catch 
        {
            Console.WriteLine("Error in communication with server");
            return false; 
        }
    }

    public async Task<bool> Update(ContactViewModel contact)
    {
        string currentToken = await tokenStorage.GetToken();
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", currentToken);
        try
        {
            var response = await httpClient.PostAsJsonAsync($"api/contacts/{contact.Id}", contact);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            Console.WriteLine("Error in communication with server");
            return false;
        }
    }

    public async Task<bool> Delete(int contactId)
    {
        string currentToken = await tokenStorage.GetToken();
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", currentToken);
        try
        {
            var response = await httpClient.DeleteAsync($"api/contacts/{contactId}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            Console.WriteLine("Error in communication with server");
            return false;
        }
    }
}
