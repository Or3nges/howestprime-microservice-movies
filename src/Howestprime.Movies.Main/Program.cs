using Howestprime.Movies.Infrastructure.Persistence.EntityFramework;
using Howestprime.Movies.Main.Modules;
using Howestprime.Movies.Main.Modules.Messaging;
using Howestprime.Movies.Main.Modules.Persistence;
using Howestprime.Movies.Main.Modules.WebApi;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.PropertyNameCaseInsensitive = true;
});

builder
    .Services
    .AddPersistenceModule(configuration)
    .AddWebApiModule(configuration)
    .AddMessagingModule(configuration)
    .AddUseCases();

var app = builder.Build();

app.UseWebApiModule();


using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();
        
        app.Logger.LogInformation("Applying database migrations...");
        dbContext.Database.Migrate();
        app.Logger.LogInformation("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while migrating the database.");
        throw;
    }
}

await app.RunMessagingModule();

await app.RunAsync();
