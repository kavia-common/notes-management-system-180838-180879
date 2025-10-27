using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NotesBackend.Models;
using NotesBackend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "Notes API";
    settings.Version = "v1";
    settings.Description = "RESTful API for managing notes (in-memory demo).";
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register repository (in-memory by default)
builder.Services.AddSingleton<INotesRepository, InMemoryNotesRepository>();

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

// Configure OpenAPI/Swagger
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
});

// Health check endpoint
app.MapGet("/", () => new { message = "Healthy" })
   .WithTags("Health");

// PUBLIC_INTERFACE
app.MapGet("/api/notes", ([FromServices] INotesRepository repo) =>
{
    /// <summary>
    /// Get all notes.
    /// </summary>
    /// <returns>200 OK with list of notes.</returns>
    var notes = repo.GetAll();
    return Results.Ok(notes);
})
.WithTags("Notes")
.Produces<IEnumerable<Note>>(StatusCodes.Status200OK)
.WithSummary("List notes")
.WithDescription("Returns a list of all notes.");

// PUBLIC_INTERFACE
app.MapGet("/api/notes/{id:guid}", ([FromServices] INotesRepository repo, Guid id) =>
{
    /// <summary>
    /// Get a single note by id.
    /// </summary>
    /// <param name="id">Note GUID</param>
    /// <returns>200 OK with note or 404 Not Found.</returns>
    var note = repo.GetById(id);
    return note is null ? Results.NotFound() : Results.Ok(note);
})
.WithTags("Notes")
.Produces<Note>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithSummary("Get note")
.WithDescription("Returns a note by its GUID.");

// PUBLIC_INTERFACE
app.MapPost("/api/notes", ([FromServices] INotesRepository repo, [FromBody] CreateNoteRequest request) =>
{
    /// <summary>
    /// Create a new note.
    /// </summary>
    /// <param name="request">Note creation payload</param>
    /// <returns>201 Created with created note or 400 Bad Request.</returns>
    if (request is null) return Results.BadRequest(new { error = "Request body is required." });

    // Basic validation
    if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Length > 256)
        return Results.BadRequest(new { error = "Title is required and must be 1-256 characters." });
    if (request.Content is { Length: > 5000 })
        return Results.BadRequest(new { error = "Content must be <= 5000 characters." });

    var note = new Note
    {
        Title = request.Title.Trim(),
        Content = request.Content
    };

    var created = repo.Create(note);
    return Results.Created($"/api/notes/{created.Id}", created);
})
.WithTags("Notes")
.Produces<Note>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest)
.WithSummary("Create note")
.WithDescription("Creates a new note and returns it.");

// PUBLIC_INTERFACE
app.MapPut("/api/notes/{id:guid}", ([FromServices] INotesRepository repo, Guid id, [FromBody] UpdateNoteRequest request) =>
{
    /// <summary>
    /// Update an existing note.
    /// </summary>
    /// <param name="id">Note GUID</param>
    /// <param name="request">Note update payload</param>
    /// <returns>200 OK with updated note, 400 Bad Request, or 404 Not Found.</returns>
    if (request is null) return Results.BadRequest(new { error = "Request body is required." });

    var existing = repo.GetById(id);
    if (existing is null) return Results.NotFound();

    if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Length > 256)
        return Results.BadRequest(new { error = "Title is required and must be 1-256 characters." });
    if (request.Content is { Length: > 5000 })
        return Results.BadRequest(new { error = "Content must be <= 5000 characters." });

    existing.Title = request.Title.Trim();
    existing.Content = request.Content;
    existing.UpdatedAt = DateTime.UtcNow;

    var ok = repo.Update(existing);
    return ok ? Results.Ok(existing) : Results.NotFound();
})
.WithTags("Notes")
.Produces<Note>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound)
.WithSummary("Update note")
.WithDescription("Updates an existing note and returns it.");

// PUBLIC_INTERFACE
app.MapDelete("/api/notes/{id:guid}", ([FromServices] INotesRepository repo, Guid id) =>
{
    /// <summary>
    /// Delete a note by id.
    /// </summary>
    /// <param name="id">Note GUID</param>
    /// <returns>204 No Content or 404 Not Found.</returns>
    var deleted = repo.Delete(id);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithTags("Notes")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound)
.WithSummary("Delete note")
.WithDescription("Deletes a note by its GUID.");

app.Run();