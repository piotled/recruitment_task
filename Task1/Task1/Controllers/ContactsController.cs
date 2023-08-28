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

    [AllowAnonymous]
    [HttpGet]
    public IEnumerable<ContactDTO> GetAllContacts()
    {
        return dbContext.Contacts.Select(c => MapContactToDTO(c));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetContactById(int id)
    {
        var foundContact = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id);

        if (foundContact is null)
            return NotFound();

        return Ok(MapContactToDTO(foundContact));
    }

    [HttpPost]
    public async Task<IActionResult> CreateContact(ContactDTO contactDto)
    {
        var contactToCreate = MapDTOToContact(contactDto);

        if (!await IsValidForInsertion(contactToCreate))
            return BadRequest();
        
        dbContext.Update(contactToCreate);
        await dbContext.SaveChangesAsync();
        
        return Ok();
    }

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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var contactToRemove = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id);
        if (contactToRemove is null)
            return NotFound();
        
        dbContext.Remove(contactToRemove);
        await dbContext.SaveChangesAsync();

        return Ok();
    }

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

    private async Task<bool> ValidateCategoryIntegrity(Contact contact)
    {
        var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == contact.CategoryId);
        var existingSubcategory = await dbContext.Subcategories.FirstOrDefaultAsync(sc => sc.Id == contact.SubcategoryId);
        if (existingCategory is null || existingSubcategory is null
            || existingCategory.Id != existingSubcategory.CategoryId)
            return false;

        return true;
    }
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

    private static Contact MapDTOToContact(ContactDTO contact)
    {
        return new Contact
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
}
