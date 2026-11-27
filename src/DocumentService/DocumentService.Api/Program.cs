
using DocumentService.Api.DTOs;
using DocumentService.Core.Models;
using DocumentService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Kết nối SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Trang chủ
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

// Health check API
app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        status = "Healthy",
        service = "Document API",
        time = DateTime.UtcNow
    });
});

// Kiểm tra kết nối database
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

// CREATE DOCUMENT
app.MapPost("/documents", async (
    CreateDocumentRequest request,
    AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.BadRequest(new
        {
            message = "Title is required"
        });
    }

    if (request.Title.Trim().Length > 200)
    {
        return Results.BadRequest(new
        {
            message = "Title cannot exceed 200 characters"
        });
    }

    if (string.IsNullOrWhiteSpace(request.CreatedBy))
    {
        return Results.BadRequest(new
        {
            message = "CreatedBy is required"
        });
    }

    if (request.CreatedBy.Trim().Length > 100)
    {
        return Results.BadRequest(new
        {
            message = "CreatedBy cannot exceed 100 characters"
        });
    }

    var document = new Document
    {
        Id = Guid.NewGuid(),
        Title = request.Title.Trim(),
        Content = request.Content,
        CreatedBy = request.CreatedBy.Trim(),
        Approver = request.Approver?.Trim(),
        Status = "Draft",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    db.Documents.Add(document);

    await db.SaveChangesAsync();

    return Results.Created(
        $"/documents/{document.Id}",
        document
    );
});

// READ ALL DOCUMENTS
app.MapGet("/documents", async (AppDbContext db) =>
{
    var documents = await db.Documents
        .AsNoTracking()
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync();

    return Results.Ok(documents);
});

// READ DOCUMENT BY ID
app.MapGet("/documents/{id:guid}", async (
    Guid id,
    AppDbContext db) =>
{
    var document = await db.Documents
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == id);

    if (document == null)
    {
        return Results.NotFound(new
        {
            message = "Document not found"
        });
    }

    return Results.Ok(document);
});

// UPDATE DOCUMENT
app.MapPut("/documents/{id:guid}", async (
    Guid id,
    UpdateDocumentRequest request,
    AppDbContext db) =>
{
    var document = await db.Documents
        .FirstOrDefaultAsync(x => x.Id == id);

    if (document == null)
    {
        return Results.NotFound(new
        {
            message = "Document not found"
        });
    }

    if (document.Status != "Draft")
    {
        return Results.Conflict(new
        {
            message = "Only Draft documents can be updated"
        });
    }

    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.BadRequest(new
        {
            message = "Title is required"
        });
    }

    if (request.Title.Trim().Length > 200)
    {
        return Results.BadRequest(new
        {
            message = "Title cannot exceed 200 characters"
        });
    }

    document.Title = request.Title.Trim();
    document.Content = request.Content;
    document.Approver = request.Approver?.Trim();
    document.UpdatedAt = DateTime.UtcNow;

    await db.SaveChangesAsync();

    return Results.Ok(document);
});

// DELETE DOCUMENT
app.MapDelete("/documents/{id:guid}", async (
    Guid id,
    AppDbContext db) =>
{
    var document = await db.Documents
        .FirstOrDefaultAsync(x => x.Id == id);

    if (document == null)
    {
        return Results.NotFound(new
        {
            message = "Document not found"
        });
    }

    if (document.Status != "Draft")
    {
        return Results.Conflict(new
        {
            message = "Only Draft documents can be deleted"
        });
    }

    var hasHistory = await db.ApprovalHistories
        .AnyAsync(x => x.DocumentId == id);

    var hasNotifications = await db.Notifications
        .AnyAsync(x => x.DocumentId == id);

    if (hasHistory || hasNotifications)
    {
        return Results.Conflict(new
        {
            message = "Document has related records and cannot be deleted"
        });
    }

    db.Documents.Remove(document);

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();