using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var vaultItems = new List<VaultItem>()
{
    new(0, "Steve"),
    new(1, "Michael"),
};

app.MapGet("/api/ping", () => new
{
    status = "OK"
});

app.MapGet("/api/vault", Ok<List<VaultItem>> () => TypedResults.Ok(vaultItems));

app.MapGet("/api/vault/{id:int}", Results<Ok<VaultItem>, NotFound> (int id) =>
{
    var item = vaultItems.Find(x => x.Id == id);
    return item is null ? TypedResults.NotFound() :  TypedResults.Ok(item);
});

app.MapPost("/api/vault", (CreateVaultItemDto dto) =>
{
    var id = Random.Shared.Next();
    while (vaultItems.Any(x => x.Id == id))
        id = Random.Shared.Next();
    
    var item = new VaultItem(id, dto.Name);
    vaultItems.Add(item);
    return TypedResults.Ok(item);
});

app.MapPut("/api/vault/{id:int}", Results<Ok<VaultItem>, NotFound> (UpdateVaultItemDto dto, int id) =>
{
    var index = vaultItems.FindIndex(x => x.Id == id);
    if (index is -1)
        return TypedResults.NotFound();
    
    return TypedResults.Ok(vaultItems[index] = new VaultItem(id, dto.Name));
});

app.MapDelete("/api/vault/{id:int}", Results<NoContent, NotFound> (int id) =>
{
    var item = vaultItems.Find(x => x.Id == id);
    if (item is null)
        return TypedResults.NotFound();
    
    vaultItems.Remove(item);
    return TypedResults.NoContent();
});

app.Run();

internal record CreateVaultItemDto(string Name);
internal record UpdateVaultItemDto(string Name);
internal record VaultItem(int Id, string Name);