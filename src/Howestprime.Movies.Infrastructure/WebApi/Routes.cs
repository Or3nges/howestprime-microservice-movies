using System.Net.Mime;
using Howestprime.Movies.Infrastructure.WebApi.Controllers;
using Howestprime.Movies.Infrastructure.WebApi.Controllers.Booking;
using Howestprime.Movies.Infrastructure.WebApi.Controllers.MovieEvents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        MapMovieRoutes(app);
        MapMovieEventRoutes(app);
        
        // Add root route redirect to swagger
        app.MapGet("/", ctx => {
            ctx.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });
        
        return app;
    }

    private static void MapMovieRoutes(WebApplication app)
    {
        var movieGroup = app.MapGroup("api/movies")
            .WithTags("Movies")
            .WithDescription("Endpoint Related to Movies")
            .WithOpenApi();


        movieGroup.MapPost("/", RegisterMovieController.Invoke)
            .WithName("RegisterMovie")
            .WithDescription("Register a new movie")
            .WithMetadata(new ConsumesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();


        movieGroup.MapGet("/", FindMoviesByFiltersController.Invoke)
            .WithName("FindMoviesByFilters")
            .WithDescription("Find movies by filters")
            .WithMetadata(new ProducesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();

        movieGroup.MapGet("/{id}", FindMovieByIdController.Invoke)
            .WithName("FindMovieById")
            .WithDescription("Find a movie by id")
            .WithMetadata(new ProducesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();
    }

    private static void MapMovieEventRoutes(WebApplication app)
    {
        var movieEventGroup = app.MapGroup("/api/movie-events")
            .WithTags("MovieEvents")
            .WithDescription("Endpoint Related to Movie Events")
            .WithOpenApi();

        movieEventGroup.MapPost("/", ScheduleMovieEventController.Invoke)
            .WithName("ScheduleMovieEvent")
            .WithDescription("Schedule a new movie event")
            .WithMetadata(new ConsumesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();

        movieEventGroup.MapGet("/", FindMovieWithEventsInTimeFrameController.Invoke)
            .WithName("FindMovieWithEventsInTimeFrame")
            .WithDescription("Find movies with events in a specific time frame")
            .WithMetadata(new ProducesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();

        movieEventGroup.MapGet("/movie", FindMovieByIdWithEventsController.Invoke)
            .WithName("FindMovieByIdWithEvents")
            .WithDescription("Find a movie by id with events")
            .WithMetadata(new ProducesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();

        movieEventGroup.MapGet("/monthly", FindMovieEventsForMonthController.Invoke)
            .WithName("FindMovieEventsForMonth")
            .WithDescription("Find movie events for a specific month")
            .WithMetadata(new ProducesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();
        
        movieEventGroup.MapPost("/{movieEventId}/bookings", BookMovieEventController.Invoke)
            .WithName("BookMovieEvent")
            .WithDescription("Book a movie event")
            .WithMetadata(new ConsumesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();

        
    }
}
