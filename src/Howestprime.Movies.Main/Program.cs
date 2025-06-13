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


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();
    

    app.Logger.LogInformation("Ensuring database is created and migrations are applied...");
    try
    {

        dbContext.Database.EnsureCreated();
        

        var connection = dbContext.Database.GetDbConnection();
        connection.Open();
        try
        {
            var migrationsTableExists = connection.GetSchema("Tables")
                .Select("TABLE_NAME = '__EFMigrationsHistory'")
                .Any();

            if (!migrationsTableExists)
            {

                app.Logger.LogInformation("Migrations table not found. Creating and applying initial migration...");
                dbContext.Database.ExecuteSqlRaw("CREATE TABLE \"__EFMigrationsHistory\" (\"MigrationId\" character varying(150) NOT NULL, \"ProductVersion\" character varying(32) NOT NULL, CONSTRAINT \"PK___EFMigrationsHistory\" PRIMARY KEY (\"MigrationId\"));");
                dbContext.Database.ExecuteSqlRaw("INSERT INTO \"__EFMigrationsHistory\" (\"MigrationId\", \"ProductVersion\") VALUES ('20250610210616_InitialSchema', '7.0.0');");
            }
            else
            {

                var appliedMigrations = dbContext.Database.GetAppliedMigrations().ToList();
                if (!appliedMigrations.Any())
                {
                    // If no migrations are applied but tables exist, mark the initial migration as applied
                    app.Logger.LogInformation("No migrations applied but tables exist. Marking initial migration as applied...");
                    dbContext.Database.ExecuteSqlRaw("INSERT INTO \"__EFMigrationsHistory\" (\"MigrationId\", \"ProductVersion\") VALUES ('20250610210616_InitialSchema', '7.0.0');");
                }
            }


            var moviesTableExists = connection.GetSchema("Tables")
                .Select("TABLE_NAME = 'Movies'")
                .Any();
                
            if (!moviesTableExists)
            {

                dbContext.Database.Migrate();
            }
        }
        finally
        {
            connection.Close();
        }
        
        app.Logger.LogInformation("Database setup complete");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while setting up the database");
        throw;
    }
}

await app.RunMessagingModule();

await app.RunAsync();
