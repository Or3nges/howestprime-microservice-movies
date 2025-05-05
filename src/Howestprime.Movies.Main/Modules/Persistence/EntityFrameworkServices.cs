using Domaincrafters.Application;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configurations;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories;
using Howestprime.Movies.Application.Contracts.Ports;

namespace Howestprime.Movies.Main.Modules.Persistence;

public static class EntityFrameworkServices
{
    public static IServiceCollection AddEntityFrameworkServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddDbContext(configuration)
            .AddRepositories()
            .AddQueries()
            .AddScoped<Howestprime.Movies.Application.Contracts.Ports.IUnitOfWork, EntityFrameworkUoW>();

        return services;
    }

    private static IServiceCollection AddDbContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string databaseProvider = configuration.GetValue<string>("Database:Provider")!;
        string connectionString = configuration.GetValue<string>("Database:ConnectionString")!;
        
        switch (databaseProvider)
        {
            case "PostgreSQL":
                services.AddDbContext<DomainContextBase, DomainContextPostgres>();
                services.AddDbContext<QueryContextBase, QueryContextPostgres>();
                
                // Register MoviesDbContext with the connection string
                services.AddDbContext<MoviesDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });
                
                // Register DbContext for EntityFrameworkUoW
                services.AddScoped<DbContext>(provider => provider.GetRequiredService<MoviesDbContext>());
                
                break;
            default:
                throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported.");
        }
        
        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services
    )
    {
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IMovieEventRepository, MovieEventRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        return services;
    }

    private static IServiceCollection AddQueries(
        this IServiceCollection services
    )
    {
        return services;
    }
    
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DomainContextBase>();

        context.Database.Migrate();
        Console.WriteLine("Database migrated.");

        return app;
    }
}