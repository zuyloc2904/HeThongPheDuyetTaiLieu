using DocumentService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

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
app.MapGet("/health/db", async (AppDbContext db) =>
{
    var connected = await db.Database.CanConnectAsync();

    if (connected)
    {
        return Results.Ok(new
        {
            status = "Connected",
            database = "DocumentApprovalDB"
        });
    }

    return Results.Problem("Cannot connect to SQL Server");
});

app.Run();