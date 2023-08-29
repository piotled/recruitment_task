using System.Net.Http.Json;

namespace FrontendBlazor.Model;

public class ContactsDAO
{
    private readonly HttpClient httpClient;

    public ContactsDAO(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("RestApi");
    }

    public async Task<List<Contact>> GetAll()
    {
        var response = await httpClient.GetFromJsonAsync<List<Contact>>("api/contacts");
        return response ?? new List<Contact>();
    }
}
