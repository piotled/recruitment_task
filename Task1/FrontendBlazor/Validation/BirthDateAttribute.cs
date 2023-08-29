using System.ComponentModel.DataAnnotations;

namespace RecruitmentTask.Frontend.Validation;

/// <summary>
/// Sprawdza czy data urodzenia jest większa niż 1900/01/01
/// </summary>
sealed class BirthDateAttribute : DataTypeAttribute
{
    public BirthDateAttribute()
        : base(DataType.Date)
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
            return true;

        if (value is DateTime date && date >= new DateTime(1900,01,01))
        {
            return true;
        }

        return false;
    }
}
