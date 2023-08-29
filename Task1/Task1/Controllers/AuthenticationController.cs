using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using RecruitmentTask.Api.DTO;
using RecruitmentTask.Api.Authorization;

namespace RecruitmentTask.Api.Controllers;

/// <summary>
/// Klasa odpowiedzialna za operacje zwi�zane z uzyskiwaniem dost�pu do API
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase 
{
    readonly UserManager<IdentityUser> userManager;
    readonly ITokenManager tokenManager;

    public AuthenticationController(UserManager<IdentityUser> userManager, ITokenManager tokenManager)
    {
        this.userManager = userManager;
        this.tokenManager = tokenManager;
    }

    /// <summary>
    /// Pozwala uzyska� token umo�liwiaj�cy dost�p do chronionych metod API.
    /// </summary>
    /// <param name="userLoginData">Dane logowania u�ytkownika</param>
    /// <returns>
    /// Status 200 przy poprawnym zalogowaniu, status 403 gdy u�ytkonwik jest tymczasowo zablokowany, 
    /// status 400 gdy podane dane logowania nie s� poprawne. 
    /// W przypadku b��du cia�o odpowiedzi zawiera kr�tki opis przyczyny.
    /// </returns>
    [HttpPost("login")]
    public async Task<IActionResult> GetAccessToken(UserDTO userLoginData)
    {
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

    /// <summary>
    /// Metoda pozwala klientom sprawdzi�, czy token jest wa�ny.
    /// </summary>
    /// <returns>Status 200 je�eli token jest wa�ny, status 401 w przeciwnym wypadku.</returns>
    [Authorize]
    [HttpGet("status")]
    public IActionResult CheckTokenStatus()
    {
        return Ok();
    }

    /// <summary>
    /// Metoda wylogowyj�ca u�ytkownika. Token u�yty do autoryzacji zostanie w bazie oznaczony jako niewa�ny.
    /// </summary>
    /// <remarks>
    /// Metoda mo�e zosta� wywo�ana wiele razy z tym samym tokenem, b�d� bez tokena.
    /// </remarks>
    /// <returns>
    /// Zawsze zwraca kod 200 dla poprawnie wykonanej operacji.
    /// </returns>
    [HttpPost("logout")]
    public async Task<IActionResult> CancelToken()
    {
        var tokenIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == "TokenId");

        if (tokenIdClaim == null)
            return Ok();

        await tokenManager.CancelToken(tokenIdClaim.Value);
        
        return Ok();
    }
}