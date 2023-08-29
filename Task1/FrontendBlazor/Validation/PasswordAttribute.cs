using System.ComponentModel.DataAnnotations;

namespace RecruitmentTask.Frontend.Validation;

/// <summary>
/// Sprawdza czy hasło spełnia wymagania złożoności
/// </summary>
sealed class PasswordAttribute : DataTypeAttribute
{
    public PasswordAttribute()
        : base(DataType.Password)
    {
    }

    public override bool IsValid(object? value)
    {
        ErrorMessage = "Hasło nie może być puste";

        if (value is null)
            return false;

        string password = value.ToString()!;

        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (password.Length < 8)
        {
            ErrorMessage = "Hasło musi mieć co najmniej 8 znaków";
            return false;
        }

        bool hasDigit = false;
        bool hasLowercase = false;
        bool hasUppercase = false;
        bool hasSpecialChar = false;

        foreach (var c in password)
        {
            if (char.IsDigit(c))
                hasDigit = true;
            if (char.IsUpper(c))
                hasUppercase = true;
            if (char.IsLower(c))
                hasLowercase = true;
            if (!char.IsDigit(c) && !char.IsLetter(c))
                hasSpecialChar = true;
        }

        if (hasDigit && hasLowercase && hasUppercase && hasSpecialChar)
            return true;
        else
        {
            ErrorMessage = "Hasło musi zawierać co najmniej jedną cyfrę, znak specjalny, dużą literę i małą literę.";
            return false;
        }
    }
}
