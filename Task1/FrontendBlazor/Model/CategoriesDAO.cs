using System.Net.Http.Json;

namespace FrontendBlazor.Model;

public class CategoriesDAO
{
    private readonly HttpClient httpClient;

    public CategoriesDAO(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("RestApi");
    }

    public async Task<List<Category>> GetAll()
    {
        var response = await httpClient.GetFromJsonAsync<List<Category>>("api/categories");
        return response ?? new List<Category>();
    }
}
