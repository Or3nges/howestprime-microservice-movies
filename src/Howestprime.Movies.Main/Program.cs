using Howestprime.Movies.Infrastructure.Persistence.EntityFramework;
using Howestprime.Movies.Main.Modules;
using Howestprime.Movies.Main.Modules.Messaging;
using Howestprime.Movies.Main.Modules.Persistence;
using Howestprime.Movies.Main.Modules.WebApi;
using Howestprime.Movies.Application.Contracts.Ports;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Configure JSON serialization
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.PropertyNameCaseInsensitive = true;
});

/**
TODO: Only add the remaining modules if they are properly configured in the appsettings.json, and other settings files.
Otherwise, the app will not start.
**/
builder
    .Services
    .AddPersistenceModule(configuration)
    .AddWebApiModule(configuration)
    .AddMessagingModule(configuration)
    .AddUseCases();

var app = builder.Build();

app.UseWebApiModule();

// Add this after builder.Build() but before app.Run()
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();
    
    // Ensure database exists and apply pending migrations
    app.Logger.LogInformation("Ensuring database is created and migrations are applied...");
    try
    {
        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();
        app.Logger.LogInformation("Database setup complete");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while setting up the database");
    }
}

await app.RunMessagingModule();

await app.RunAsync();
