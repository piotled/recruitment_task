using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Task1.DataAccess;

namespace Task1.Authorization;

public class TokenNotCancelledAuthorizationHandler : IAuthorizationHandler, IAuthorizationRequirement
{
    private readonly AppDbContext dbContext;

    public TokenNotCancelledAuthorizationHandler(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var tokenIdClaim = context.User.Claims.FirstOrDefault(claim =>
                claim.Type == "TokenId"
            );

        if (tokenIdClaim == null)
            return Task.CompletedTask;

        var tokenId = tokenIdClaim.Value;

        var issuedToken = dbContext.IssuedTokens.FirstOrDefault(issuedToken =>
                issuedToken.Id.ToString() == tokenId
            );

        if (issuedToken == null)
            return Task.CompletedTask;

        if (issuedToken.IsCancelled)
            context.Fail();
        else
            context.Succeed(this);

        return Task.CompletedTask;
    }
}
