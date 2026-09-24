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

app.MapPost("/api/vault", (VaultItem item) =>
{
    vaultItems.Add(item);
    return TypedResults.Ok(item);
});

app.Run();

internal record VaultItem(int Id, string Name);