using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentTask.Api.DataAccess;
using RecruitmentTask.Api.DTO;

namespace RecruitmentTask.Api.Controllers;

/// <summary>
/// Klasa pozwalająca na wykonywanie operacji CRUD na kategoriach i podkategoriach
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private const int categoryOtherId = 1; //Identyfikator kategorii "Inne"

    private readonly AppDbContext dbContext;

    public CategoriesController(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Zwraca listę wszystkich kategorii
    /// </summary>
    [HttpGet]
    public IEnumerable<CategoryDTO> GetCategories()
    {
        return dbContext.Categories.Select(c => new CategoryDTO { Id = c.Id, Name = c.Name });
    }

    /// <summary>
    /// Zwraca listę podkategorii dla kategorii o podanym identyfikatorze
    /// </summary>
    [HttpGet("{id:int}/subcategories")]
    public IEnumerable<SubcategoryDTO> GetSubcategoriesOfGivenCategory(int id)
    {
        return dbContext.Subcategories.Where(sc => sc.CategoryId == id)
            .Select(c => new SubcategoryDTO { Id = c.Id, Name = c.Name, CategoryId = id });
    }

    /// <summary>
    /// Tworzy nową kategorię "Inne" jako podkategorię.
    /// </summary>
    /// <param name="subcategoryDto">Dane podkategorii "Inne".</param>
    /// <returns>
    /// Status 200 i identyfikator utworzonej podkategorii w przypadku sukcesu, status 400 w przypadku gdy 
    /// identyfikator kategorii otrzymany w danych subkategorii nie wskazuje na kategorię "Inne"
    /// </returns>
    [Authorize]
    [HttpPost("other")]
    public async Task<IActionResult> CreateOtherCategory(SubcategoryDTO subcategoryDto)
    {
        if (subcategoryDto.CategoryId != categoryOtherId)
            return BadRequest();

        var otherCategoryToCreate = new Subcategory
        {
            Name = subcategoryDto.Name,
            CategoryId = subcategoryDto.CategoryId,
        };

        dbContext.Update(otherCategoryToCreate);
        await dbContext.SaveChangesAsync();

        return Ok(otherCategoryToCreate.Id);
    }
}
