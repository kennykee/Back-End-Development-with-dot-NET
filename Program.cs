using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestMethod
                            | HttpLoggingFields.RequestPath
                            | HttpLoggingFields.RequestHeaders
                            | HttpLoggingFields.RequestBody
                            | HttpLoggingFields.ResponseStatusCode
                            | HttpLoggingFields.ResponseHeaders
                            | HttpLoggingFields.ResponseBody;

    // Optional: limit body size to avoid huge logs
    logging.RequestBodyLogLimit = 4096; // 4 KB
    logging.ResponseBodyLogLimit = 4096;

    // Optional: redact sensitive headers
    logging.MediaTypeOptions.AddText("application/json");
    logging.RequestHeaders.Add("Authorization");
});

var app = builder.Build();

app.UseHttpLogging();

// Custom logging middleware
app.Use(async (context, next) =>
{
    var logger = app.Logger;
    logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");
    await next();
    logger.LogInformation($"Response: {context.Response.StatusCode}");
});


// Simple authentication middleware for /users endpoints
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/users"))
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        const string validToken = "Bearer mysecrettoken";
        if (authHeader != validToken)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }
    }
    await next();
});

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

// Mockup user list
var users = new List<User>
{
    new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
    new User { Id = 2, Name = "Bob", Email = "bob@example.com" },
    new User { Id = 3, Name = "Charlie", Email = "charlie@example.com" }
};

// CRUD Endpoints for User Management
app.MapGet("/users", () => users);

app.MapGet("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});


app.MapPost("/users", (User user) =>
{
    var validation = UserValidator.ValidateUser(user);
    if (!validation.IsValid)
        return Results.BadRequest(validation.ErrorMessage);
    user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
    users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
});


app.MapPut("/users/{id}", (int id, User updatedUser) =>
{
    var validation = UserValidator.ValidateUser(updatedUser);
    if (!validation.IsValid)
        return Results.BadRequest(validation.ErrorMessage);
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();
    user.Name = updatedUser.Name;
    user.Email = updatedUser.Email;
    return Results.Ok(user);
});

app.MapDelete("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();
    users.Remove(user);
    return Results.NoContent();
});

app.Run();

static class UserValidator
{
    public static (bool IsValid, string? ErrorMessage) ValidateUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Name))
            return (false, "Name is required.");
        if (user.Name.Length < 2 || user.Name.Length > 50)
            return (false, "Name must be between 2 and 50 characters.");
        if (string.IsNullOrWhiteSpace(user.Email))
            return (false, "Email is required.");
        if (!System.Text.RegularExpressions.Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return (false, "Email is not valid.");
        return (true, null);
    }
}

class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
