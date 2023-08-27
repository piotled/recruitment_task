using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Task1.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Task1.Authorization;

public class TokenManager
{
    readonly JwtTokenOptions tokenOptions;
    readonly AppDbContext dbContext;

    public TokenManager(IOptionsSnapshot<JwtTokenOptions> tokenOptionsSnapshot,
                        AppDbContext dbContext)
    {
        tokenOptions = tokenOptionsSnapshot.Value;
        this.dbContext = dbContext;
    }

    public Task CancelToken(string tokenId)
    {
        dbContext.IssuedTokens.First(issuedToken =>
            issuedToken.Id.ToString() == tokenId
        ).IsCancelled = true;

        return dbContext.SaveChangesAsync();
    }

    public async Task<string> CreateToken(string userId)
    {
        var issuedTokenEntity = new IssuedToken();
        dbContext.IssuedTokens.Add(issuedTokenEntity);
        await dbContext.SaveChangesAsync(); 

        var handler = new JwtSecurityTokenHandler();
        var signingKeyBytes = Encoding.UTF8.GetBytes(tokenOptions.SigningKey);
        var token = handler.CreateJwtSecurityToken(new SecurityTokenDescriptor()
        {
            Claims = new Dictionary<string, object> {
                { "sub", userId },
                { "aud", tokenOptions.Audience },
                { "iss", tokenOptions.Issuer },
                { "TokenId", issuedTokenEntity.Id}
            },
            Expires = DateTime.Now + tokenOptions.AccessTokenValidityDuration,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKeyBytes),
                                                        SecurityAlgorithms.HmacSha512Signature)
        });
        return handler.WriteToken(token);
    }
}
