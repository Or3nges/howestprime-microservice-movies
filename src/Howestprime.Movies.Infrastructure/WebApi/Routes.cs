using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

namespace Howestprime.Movies.Infrastructure.WebApi;

public static class Routes
{
    public static OpenApiInfo OpenApiInfo { get; } = new OpenApiInfo
    {
        Version = "v1",
        Title = "Movies API",
        Description = "A simple API to manage movies and movie events.",
        Contact = new OpenApiContact
        {
            Name = "Matthias Blomme",
            Email = "matthias.blomme@howest.be"
        }
    };

    public static WebApplication MapRoutes(this WebApplication app)
    {
        return app;
    }
}
