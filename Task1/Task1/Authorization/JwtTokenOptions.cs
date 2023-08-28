using System.ComponentModel.DataAnnotations;

namespace RecruitmentTask.Api.Authorization;

/// <summary>
/// Konfiguracja wartości pól tokenu, przeznaczona do użycia z dotnetowym frameworkiem
/// </summary>
/// <remarks>
/// Pola <see cref="Audience"/> i <see cref="Issuer"/> mogą zawierać dowolną wartość, 
/// jako że użyty jest bardzo prosty mechanizm sprawdzania tokenu. 
/// </remarks>
public class JwtTokenOptions
{
    public const string SectionName = "Token";
    
    [Required]
    public string Audience { get; set; } = string.Empty;
    
    [Required]
    public string Issuer { get; set; } = string.Empty;
    
    /// <summary>
    /// Sekret użyty do podpisu tokenu
    /// </summary>
    [Required]
    public string SigningKey { get; set; } = string.Empty;
    
    /// <summary>
    /// Czas ważności tokenu
    /// </summary>
    [Required]
    public TimeSpan AccessTokenValidityDuration { get; set; }
}
