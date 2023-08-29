using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using RecruitmentTask.Api.DTO;
using RecruitmentTask.Api.DataAccess;

namespace RecruitmentTask.Api.Controllers;

/// <summary>
/// Klasa pozwalająca na wykonywanie operacji CRUD na kontaktach
/// </summary>
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly AppDbContext dbContext;

    public ContactsController(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Zwraza listę wszysktich kontaktów
    /// </summary>
    [AllowAnonymous]
    [HttpGet]
    public IEnumerable<ContactDTO> GetAllContacts()
    {
        return dbContext.Contacts.Select(c => MapContactToDTO(c));
    }

    /// <summary>
    /// Zwrace dane kontaktu o podanym identyfikatorze
    /// </summary>
    /// <param name="id">Identyfikator kontatku do znalezienia</param>
    /// <returns>Status 200 w przypadku sukcesu, status 404 jeżeli nie znaleziono kontaktu</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetContactById(int id)
    {
        var foundContact = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id);

        if (foundContact is null)
            return NotFound();

        return Ok(MapContactToDTO(foundContact));
    }

    /// <summary>
    /// Tworzy kontakt wykorzysując otrzymane dane.
    /// </summary>
    /// <param name="contactDto">Dane kontaktu do dodania</param>
    /// <returns>
    /// Status 200 w przypadku sukcesu, status 400 jeżeli email nie jest unikalny bądź identyfikatory kategorii są nieprawidłowe.
    /// Zwraca w ciele odpowiedzi identyfikator utworzonego kontaktu.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> CreateContact(ContactDTO contactDto)
    {
        var contactToCreate = MapDTOToContact(contactDto);

        if (!await IsValidForInsertion(contactToCreate))
            return BadRequest();
        
        dbContext.Update(contactToCreate);
        await dbContext.SaveChangesAsync();
        
        return Ok(contactToCreate.Id);
    }

    /// <summary>
    /// Modyfikuje kontakt o podanym id.
    /// </summary>
    /// <param name="contactDto">Dane kontaktu do zapisania w bazie</param>
    /// <returns>
    /// Status 200 w przypadku sukcesu, status 400 jeżeli email nie jest unikalny bądź identyfikatory kategorii są nieprawidłowe.
    /// </returns>
    [HttpPost("{id:int}")]
    public async Task<IActionResult> EditContact(ContactDTO contactDto)
    {
        var contactToChange = MapDTOToContact(contactDto);

        if (!await IsValidForChange(contactToChange))
            return BadRequest();

        dbContext.Update(contactToChange);
        await dbContext.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Usuwa kontakt wraz z podkategorią, jeżeli była ona powiązana z kategorią "Inne"
    /// </summary>
    /// <param name="id">Id kontaktu do usunięcia</param>
    /// <returns>
    /// Status 200 w przypadku sukcesu, status 404 jeżeli nie znaleziono kontaktu o podanym identyfikatorze
    /// </returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var contactToRemove = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id);
        if (contactToRemove is null)
            return NotFound();
        
        dbContext.Remove(contactToRemove);

        const int categoryOtherId = 1;
        if (contactToRemove.CategoryId == categoryOtherId)
        {
            var otherCategoryToRemove = dbContext.Subcategories.First(sc => sc.Id == categoryOtherId);
            dbContext.Remove(otherCategoryToRemove);
        }

        await dbContext.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Sprawdza czy dany kontakt spełnia warunki wymagane do dodania go do bazy
    /// </summary>
    private async Task<bool> IsValidForInsertion(Contact contact)
    {
        if (!string.IsNullOrWhiteSpace(contact.Email))
        {
            var existingContact = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Email == contact.Email);
            if (existingContact is not null)
                return false;
        }

        return await ValidateCategoryIntegrity(contact);
    }

    /// <summary>
    /// Sprawdza czy dany kontakt spełnia warunki wymagane do użycia go do zmiany danych w bazie
    /// </summary>
    private async Task<bool> IsValidForChange(Contact contact)
    {
        if (!string.IsNullOrWhiteSpace(contact.Email))
        {
            var existingContact = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Email == contact.Email);
            if (existingContact is not null && existingContact.Id != contact.Id)
                return false;
        }

        return await ValidateCategoryIntegrity(contact);
    }

    /// <summary>
    /// Sprawdza czy obiekt spełnia warunki integralności kategorii i podkategorii.
    /// </summary>
    /// <remarks>
    /// Zarówno kategoria i podkategoria musi istnieć, oraz podkategoria musi należeć do kategorii
    /// </remarks>
    /// <returns>True jeżeli warunki są spełnione, false w przeciwnym wypadku.</returns>
    private async Task<bool> ValidateCategoryIntegrity(Contact contact)
    {
        var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == contact.CategoryId);
        var existingSubcategory = await dbContext.Subcategories.FirstOrDefaultAsync(sc => sc.Id == contact.SubcategoryId);
        if (existingCategory is null || existingSubcategory is null
            || existingCategory.Id != existingSubcategory.CategoryId)
            return false;

        return true;
    }

    /// <summary>
    /// Mapuje obiekt kontaktu warstwy ORM na obiekt użytu do transferu danych pomiędzy serwerem a klientem.
    /// </summary>
    private static ContactDTO MapContactToDTO(Contact contact) 
    {
        return new ContactDTO
        {
            Id = contact.Id,
            CategoryId = contact.CategoryId,
            SubcategoryId = contact.SubcategoryId,
            DateOfBirth = contact.DateOfBirth,
            Email = contact.Email,
            Name = contact.Name,
            Surname = contact.Surname,
            Phone = contact.Phone,
        };
    }

    /// <summary>
    /// Mapuje obiekt kontaktu użytu do transferu danych pomiędzy serwerem a klientem na obiekt warstwy ORM.
    /// </summary>
    private static Contact MapDTOToContact(ContactDTO contact)
    {
        return new Contact
        {
            Id = contact.Id,
            CategoryId = contact.CategoryId,
            SubcategoryId = contact.SubcategoryId,
            DateOfBirth = DateTime.SpecifyKind(contact.DateOfBirth, DateTimeKind.Utc),
            Email = contact.Email,
            Name = contact.Name,
            Surname = contact.Surname,
            Phone = contact.Phone,
        };
    }
}
