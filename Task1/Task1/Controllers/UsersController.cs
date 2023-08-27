using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task1.DTO;

[ApiController]
[Route("api/[controller]/[action]")]
public class UsersController : ControllerBase
{
    readonly UserManager<IdentityUser> userManager;

    public UsersController(UserManager<IdentityUser> userManager)
    {
        this.userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserDTO userRegistrationData)
    {
        if (string.IsNullOrWhiteSpace(userRegistrationData.Email)
            || string.IsNullOrWhiteSpace(userRegistrationData.Password))
        {
            return BadRequest("Email or password is empty");
        }

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