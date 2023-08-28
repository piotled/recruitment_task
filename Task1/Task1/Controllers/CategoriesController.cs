using Microsoft.AspNetCore.Mvc;
using RecruitmentTask.Api.DataAccess;
using RecruitmentTask.Api.DTO;

namespace RecruitmentTask.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext dbContext;

    public CategoriesController(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    public IEnumerable<CategoryDTO> GetCategories()
    {
        return dbContext.Categories.Select(c => new CategoryDTO { Id = c.Id, Name = c.Name });
    }

    [HttpGet("{id:int}/subcategories")]
    public IEnumerable<SubcategoryDTO> GetSubcategoriesOfGivenCategory(int id)
    {
        return dbContext.Subcategories.Where(sc => sc.CategoryId == id)
            .Select(c => new SubcategoryDTO { Id = c.Id, Name = c.Name, CategoryId = id });
    }
}
