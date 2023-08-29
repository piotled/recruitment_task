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
        var response = await httpClient.GetFromJsonAsync<List<Contact>>("api/contacts");
        return response ?? new List<Contact>();
    }

    public async Task<bool> Create(Contact contact)
    {
        string currentToken = await tokenStorage.GetToken();
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", currentToken);
        var response = await httpClient.PostAsJsonAsync("api/contacts", contact);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Delete(int contactId)
    {
        string currentToken = await tokenStorage.GetToken();
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", currentToken);
        var response = await httpClient.DeleteAsync($"api/contacts/{contactId}");
        return response.IsSuccessStatusCode;
    }
}
