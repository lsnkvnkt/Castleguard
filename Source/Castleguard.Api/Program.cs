using Castleguard.Api;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IVaultService, VaultService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/ping", () => new
{
    status = "OK"
});

app.MapGet("/api/vault", Ok<List<VaultItem>> (IVaultService vaultService) =>
{
    var items = vaultService.GetVaultItems();
    return TypedResults.Ok(items);
});

app.MapGet("/api/vault/{id:int}", Results<Ok<VaultItem>, NotFound> (IVaultService vaultService, int id) =>
{
    var item = vaultService.GetVaultItem(id);
    return item is null ? TypedResults.NotFound() :  TypedResults.Ok(item);
});

app.MapPost("/api/vault", Ok<VaultItem> (IVaultService vaultService, CreateVaultItemDto dto) =>
{
    var item = vaultService.CreateVaultItem(dto.Name);
    return TypedResults.Ok(item);
});

app.MapPut("/api/vault/{id:int}", Results<Ok<VaultItem>, NotFound> (IVaultService vaultService, UpdateVaultItemDto dto, int id) =>
{
    var item = vaultService.EditVaultItemName(id, dto.Name);
    return item is null ? TypedResults.NotFound() : TypedResults.Ok(item);
});

app.MapDelete("/api/vault/{id:int}", Results<NoContent, NotFound> (IVaultService vaultService, int id) =>
{
    var item = vaultService.DeleteVaultItem(id);
    return item is null ? TypedResults.NotFound() : TypedResults.NoContent();
});

app.Run();

internal record CreateVaultItemDto(string Name);
internal record UpdateVaultItemDto(string Name);