using Microsoft.AspNetCore.OpenApi;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        system = "Document Approval System",
        service = "Document API",
        status = "Running",
        version = "1.0"
    });
});

app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        status = "Healthy",
        service = "Document API",
        time = DateTime.UtcNow
    });
});

app.Run();