using FrontendBlazor.Authentication;
using System.Net.Http.Json;

namespace FrontendBlazor.Model;

public class CategoriesDAO
{
    private const int categoryOtherId = 1;

    private readonly HttpClient httpClient;
    private readonly ITokenStorage tokenStorage;

    public CategoriesDAO(IHttpClientFactory httpClientFactory, ITokenStorage tokenStorage)
    {
        this.httpClient = httpClientFactory.CreateClient("RestApi");
        this.tokenStorage = tokenStorage;
    }

    public async Task<List<Category>> GetAll()
    {
        try
        {
            var categories = await httpClient.GetFromJsonAsync<List<Category>>("api/categories");

            if (categories is not null && categories.Count > 0)
            {
                foreach (var category in categories)
                {

                    var address = $"api/categories/{category.Id}/subcategories";
                    category.Subcategories = await httpClient.GetFromJsonAsync<List<Subcategory>>(address)
                        ?? new();

                    if (category.Id != categoryOtherId && category.Subcategories.Count == 0)
                        return new();
                }
            }
         
            return categories ?? new();
        }
        catch { }

        return new();
    }

    public async Task<int> AddOtherCategory(string categoryName)
    {
        var subcategoryDto = new Subcategory()
        {
            Name = categoryName,
            CategoryId = categoryOtherId
        };

        try
        {
            string currentToken = await tokenStorage.GetToken();
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", currentToken);
            var result = await httpClient.PostAsJsonAsync("api/categories/other", subcategoryDto);
            return await result.Content.ReadFromJsonAsync<int>();
        }
        catch 
        {
            return 0;
        }
    }
}
