namespace ShiftsLogger.Client.TerrenceLGee.Validation;

public static class PasswordValidation
{
    public static bool IsValidPassword(string password)
    {
        if (password.Length < 8) return false;
        if (!IsLowerCase(password)) return false;
        if (!IsUpperCase(password)) return false;
        if (!IsNumeric(password)) return false;
        if (!IsNonAlphanumeric(password)) return false;

        return true;
    }

    private static bool IsLowerCase(string password)
    {
        foreach (var letter in password)
        {
            if (Char.IsLower(letter))
                return true;
        }

        return false;
    }

    private static bool IsUpperCase(string password)
    {
        foreach (var letter in password)
        {
            if (Char.IsUpper(letter))
                return true;
        }

        return false;
    }

    private static bool IsNumeric(string password)
    {
        foreach (var letter in password)
        {
            if (Char.IsDigit(letter))
                return true;
        }

        return false;
    }

    private static bool IsNonAlphanumeric(string password)
    {
        foreach (var letter in password)
        {
            if (Char.IsPunctuation(letter))
                return true;
        }

        return false;
    }
}
