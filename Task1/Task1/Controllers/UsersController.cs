using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using RecruitmentTask.Api.DTO;

namespace RecruitmentTask.Api.Controllers;

/// <summary>
/// Klasa odpowiedzialna za zarządzanie użytkownikami
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class UsersController : ControllerBase
{
    readonly UserManager<IdentityUser> userManager;

    public UsersController(UserManager<IdentityUser> userManager)
    {
        this.userManager = userManager;
    }

    /// <summary>
    /// Tworzy nowe konto użytkownika.
    /// </summary>
    /// <param name="userRegistrationData">Dane logowania użytkownika</param>
    /// <returns>
    /// Status 200 jeżeli użytkownik został utworzony poprawnie, 
    /// status 400 gdy dane nie pozwalają na utworzenie użytkownika.
    /// W przypadku błędu ciało odpowiedzi zawiera krótki opis przyczyny.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> Create(UserDTO userRegistrationData)
    {
        if(await userManager.FindByEmailAsync(userRegistrationData.Email) is not null)
            return BadRequest("Provided email is already in use");

        var newUser = new IdentityUser(userRegistrationData.Email)
        {
            Email = userRegistrationData.Email,
        };

        var result = await userManager.CreateAsync(newUser, userRegistrationData.Password);

        if (!result.Succeeded)
            return BadRequest("Email is in incorrect format " +
                "or password does not satisfy complexity requirements");

        return Ok();
    }
}