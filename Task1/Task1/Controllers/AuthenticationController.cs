using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Task1.Authorization;
using Task1.DTO;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthenticationController : ControllerBase 
{
    readonly UserManager<IdentityUser> userManager;
    readonly ITokenManager tokenManager;

    public AuthenticationController(UserManager<IdentityUser> userManager, ITokenManager tokenManager)
    {
        this.userManager = userManager;
        this.tokenManager = tokenManager;
    }

    [HttpPost]
    public async Task<IActionResult> AccessToken(UserDTO userLoginData)
    {
        if (string.IsNullOrWhiteSpace(userLoginData.Email))
            return BadRequest("Email was not provided");

        if (string.IsNullOrWhiteSpace(userLoginData.Password))
            return BadRequest("Password was not provided");

        IdentityUser? user = await userManager.FindByEmailAsync(userLoginData.Email);

        if (user is null)
            return BadRequest("Invalid credentials");

        if (await userManager.IsLockedOutAsync(user))
            return Problem(statusCode: StatusCodes.Status403Forbidden, detail: "User is locked out");

        if (!await userManager.CheckPasswordAsync(user, userLoginData.Password))
        {
            await userManager.AccessFailedAsync(user);
            return BadRequest("Invalid credentials");
        }

        await userManager.ResetAccessFailedCountAsync(user);

        return Ok(await tokenManager.CreateToken(user.Id.ToString()));
    }

    [Authorize]
    [HttpGet]
    public IActionResult Status()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        var tokenIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == "TokenId");

        if (tokenIdClaim == null)
            return Ok();

        await tokenManager.CancelToken(tokenIdClaim.Value);
        
        return Ok();
    }
}