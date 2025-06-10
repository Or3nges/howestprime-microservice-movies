using Howestprime.Movies.Infrastructure.WebApi;
using Howestprime.Movies.Infrastructure.WebApi.Shared.Extensions;
using Howestprime.Movies.Infrastructure.WebApi.Controllers;
using Microsoft.OpenApi.Models;

namespace Howestprime.Movies.Main.Modules.WebApi;

public static class WebApiModule
{
    public static IServiceCollection AddWebApiModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddHealthChecks();

        return services
            .AddProblemDetails()
            .AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithExposedHeaders("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            })
            .AddOpenApiInfo(
                BuildOpenApiInfo(configuration),
                [configuration["WebApi:applicationUrl"]]);

    }

    public static WebApplication UseWebApiModule(this WebApplication app)
    {
        app.UseCors();
        if (!app.Environment.IsProduction())
            app.UseSwagger().UseSwaggerUI();
        
        app
            .UseHttpsRedirection();
                app.MapHealthChecks("/health");

        return app.MapRoutes();
    }

    private static OpenApiInfo BuildOpenApiInfo(
        this IConfiguration configuration
    )
    {
        return new OpenApiInfo
        {
            Version = configuration["WebApi:Version"],
            Title = configuration["WebApi:Title"],
            Description = configuration["WebApi:Description"],
            Contact = new OpenApiContact
            {
                Name = configuration["WebApi:Contact:Name"],
                Email = configuration["WebApi:Contact:Email"]
            },
        };
    }
}