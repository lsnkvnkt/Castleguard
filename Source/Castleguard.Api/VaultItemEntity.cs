namespace Castleguard.Api;

public class VaultItemEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public static bool IsValidName(string? name)
        => !string.IsNullOrWhiteSpace(name) && (name.Length <= 64);
}