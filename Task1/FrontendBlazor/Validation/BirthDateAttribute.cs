using System.ComponentModel.DataAnnotations;

namespace FrontendBlazor.Validation;

sealed class BirthDateAttribute : DataTypeAttribute
{
    public BirthDateAttribute()
        : base(DataType.Password)
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
