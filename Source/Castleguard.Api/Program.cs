using Castleguard.Api;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<IVaultService, VaultService>();
builder.Services.AddDbContext<CastleguardDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Castleguard")));

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

app.MapGet("/api/vault", async Task<Ok<List<VaultItem>>> (IVaultService vaultService) =>
{
    var items = await vaultService.GetVaultItemsAsync();
    return TypedResults.Ok(items);
});

app.MapGet("/api/vault/{id:int}", async Task<Results<Ok<VaultItem>, NotFound>> (IVaultService vaultService, int id) =>
{
    var item = await vaultService.GetVaultItemAsync(id);
    return item is null ? TypedResults.NotFound() : TypedResults.Ok(item);
});

app.MapPost("/api/vault", async Task<Results<Ok<VaultItem>, BadRequest<string>>> (IVaultService vaultService, CreateVaultItemDto dto) =>
{
    var validationResult = VaultItemValidator.ValidateName(dto.Name);
    if (!validationResult.IsValid)
        return TypedResults.BadRequest(validationResult.ErrorMessage);
    
    var item = await vaultService.CreateVaultItemAsync(dto.Name!);
    return TypedResults.Ok(item);
});

app.MapPut("/api/vault/{id:int}", async Task<Results<Ok<VaultItem>, NotFound, BadRequest<string>>> (IVaultService vaultService, UpdateVaultItemDto dto, int id) =>
{
    var validationResult = VaultItemValidator.ValidateName(dto.Name);
    if (!validationResult.IsValid)
        return TypedResults.BadRequest(validationResult.ErrorMessage);
    
    var item = await vaultService.EditVaultItemNameAsync(id, dto.Name!);
    return item is null ? TypedResults.NotFound() : TypedResults.Ok(item);
});

app.MapDelete("/api/vault/{id:int}", async Task<Results<NoContent, NotFound>> (IVaultService vaultService, int id) =>
{
    var item = await vaultService.DeleteVaultItemAsync(id);
    return item is null ? TypedResults.NotFound() : TypedResults.NoContent();
});

app.Run();

internal record CreateVaultItemDto(string? Name);
internal record UpdateVaultItemDto(string? Name);