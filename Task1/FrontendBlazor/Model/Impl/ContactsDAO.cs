using FrontendBlazor.Authentication;
using System.Net.Http.Json;

namespace FrontendBlazor.Model.Impl;

public class ContactsDAO : IContactsDAO
{
    private readonly HttpClient httpClient;
    private readonly ITokenStorage tokenStorage;

    public ContactsDAO(IHttpClientFactory httpClientFactory, ITokenStorage tokenStorage)
    {
        httpClient = httpClientFactory.CreateClient("RestApi");
        this.tokenStorage = tokenStorage;
    }

    public async Task<List<Contact>> GetAll()
    {
        try
        {
            return await httpClient.GetFromJsonAsync<List<Contact>>("api/contacts") ?? new();
        }
        catch
        {
            Console.WriteLine("Error in communication with server");
            return new();
        }
    }

    public async Task<Contact?> Get(int id)
    {
        try
        {
            string currentToken = await tokenStorage.GetToken();
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", currentToken);
            return await httpClient.GetFromJsonAsync<Contact>($"api/contacts/{id}");
        }
        catch
        {
            Console.WriteLine("Error in communication with server");
            return null;
        }
    }

    public async Task<bool> Create(Contact contact)
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

    public async Task<bool> Update(Contact contact)
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
