using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Howestprime.Movies.Infrastructure.WebApi.Controllers;

namespace Howestprime.Movies.Infrastructure.WebApi;

public static class Routes
{
    public static WebApplication MapRoutes(this WebApplication app)
    {
        var movies = app.MapGroup("/api/movies")
            .WithTags("Movies")
            .WithOpenApi();

        movies.MapPost("/", RegisterMovieController.Invoke)
            .WithName("RegisterMovie")
            .WithDescription("Registers a new movie")
            .WithMetadata(new ConsumesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();

        movies.MapGet("/search", FindMoviesByFiltersController.Invoke)
            .WithName("FindMoviesByFilters")
            .WithDescription("Find movies by filters")
            .WithMetadata(new ProducesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();

        movies.MapGet("/{id}", FindMovieByIdController.Invoke)
            .WithName("FindMovieById")
            .WithDescription("Find movie by ID")
            .WithOpenApi();

        movies.MapGet("/{id}/events", FindMovieByIdWithEventsController.Invoke)
            .WithName("FindMovieByIdWithEvents")
            .WithDescription("Find movie by ID with events")
            .WithOpenApi();

        var movieEvents = app.MapGroup("/api/movie-events")
            .WithTags("MovieEvents")
            .WithOpenApi();

        movieEvents.MapPost("/", ScheduleMovieEventController.Invoke)
            .WithName("ScheduleMovieEvent")
            .WithDescription("Schedules a new movie event")
            .WithMetadata(new ConsumesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();

        movieEvents.MapGet("/monthly", FindMovieEventsForMonthController.Invoke)
            .WithName("FindMovieEventsForMonth")
            .WithDescription("Find movie events for a specific month")
            .WithOpenApi();

        movieEvents.MapGet("/timeframe", FindMoviesWithEventsInTimeframeController.Invoke)
            .WithName("FindMoviesWithEventsInTimeframe")
            .WithDescription("Find movie events in a timeframe")
            .WithOpenApi();

        movieEvents.MapPost("/book", BookMovieEventController.Invoke)
            .WithName("BookMovieEvent")
            .WithDescription("Book a movie event")
            .WithMetadata(new ConsumesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();

        app.MapGet("/", ctx => {
            ctx.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });

        return app;
    }
}
