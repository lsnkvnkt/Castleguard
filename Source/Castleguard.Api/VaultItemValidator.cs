namespace Castleguard.Api;

public static class VaultItemValidator
{
    private const int MaxNameLength = 64;
    
    public static ValidationResult ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new ValidationResult(false, "Name must not be empty.");

        return name.Length > MaxNameLength
            ? new ValidationResult(false, $"Name must not exceed {MaxNameLength} characters.")
            : new ValidationResult(true, null);
    }

    public readonly record struct ValidationResult(bool IsValid, string? ErrorMessage);
}