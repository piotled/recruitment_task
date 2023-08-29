namespace FrontendBlazor.Model;

public interface ICategoriesDAO
{
    Task<int> AddOtherCategory(string categoryName);
    Task<List<Category>> GetAll();
}
