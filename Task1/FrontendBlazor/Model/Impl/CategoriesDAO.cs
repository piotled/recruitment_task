using RecruitmentTask.Frontend.Authentication;
using System.Net.Http.Json;

namespace RecruitmentTask.Frontend.Model.Impl;

public class CategoriesDAO : ICategoriesDAO
{
    private const int categoryOtherId = 1;

    private readonly HttpClient httpClient;
    private readonly ITokenStorage tokenStorage;

    public CategoriesDAO(IHttpClientFactory httpClientFactory, ITokenStorage tokenStorage)
    {
        httpClient = httpClientFactory.CreateClient("RestApi");
        this.tokenStorage = tokenStorage;
    }

    public async Task<List<CategoryViewModel>> GetAll()
    {
        try
        {
            var categories = await httpClient.GetFromJsonAsync<List<CategoryViewModel>>("api/categories");

            if (categories is not null && categories.Count > 0)
            {
                foreach (var category in categories)
                {

                    var address = $"api/categories/{category.Id}/subcategories";
                    category.Subcategories = await httpClient.GetFromJsonAsync<List<SubcategoryViewModel>>(address)
                        ?? new();

                    if (category.Id != categoryOtherId && category.Subcategories.Count == 0)
                        return new();
                }
            }

            return categories ?? new();
        }
        catch 
        {
            Console.WriteLine("Error in communication with server");
            return new();
        }
    }

    public async Task<int> AddOtherCategory(string categoryName)
    {
        var subcategoryDto = new SubcategoryViewModel()
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
            Console.WriteLine("Error in communication with server");
            return 0;
        }
    }
}
