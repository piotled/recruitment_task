using System.ComponentModel.DataAnnotations;

namespace Task1.Authorization;

public class JwtTokenOptions
{
    public const string SectionName = "Token";
    [Required]
    public string Audience { get; set; } = string.Empty;
    [Required]
    public string Issuer { get; set; } = string.Empty;
    [Required]
    public string SigningKey { get; set; } = string.Empty;
    [Required]
    public TimeSpan AccessTokenValidityDuration { get; set; }
}
