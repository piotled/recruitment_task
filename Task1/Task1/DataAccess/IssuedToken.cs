namespace RecruitmentTask.Api.DataAccess;

/// <summary>
/// Klasa służy do zapisywania informacji o wystawionych tokenach
/// w celu zapenwienia możliwości ich unieważnianai (wylogowania)
/// </summary>
public class IssuedToken
{
    public long Id { get; set; }
    public bool IsCancelled { get; set; }
}
