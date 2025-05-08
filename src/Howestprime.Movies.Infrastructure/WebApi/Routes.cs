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
            .WithDescription("Register a new movie")
            .Produces<object>(StatusCodes.Status201Created)
            .WithMetadata(new ConsumesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();

        movies.MapGet("/", FindMoviesByFiltersController.Invoke)
            .WithName("FindMoviesByFilters")
            .WithDescription("List movies by Title & Genre")
            .WithMetadata(new ProducesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();

        movies.MapGet("/{id}", FindMovieByIdController.Invoke)
            .WithName("FindMovieById")
            .WithDescription("Find a movie by ID")
            .WithOpenApi();

        var movieEvents = app.MapGroup("/api/movie-events")
            .WithTags("MovieEvents")
            .WithOpenApi();

        movieEvents.MapGet("/", FindMoviesWithEventsInTimeframeController.Invoke)
            .WithName("FindMoviesWithEventsInTimeframe")
            .WithDescription("List all movie events per movie in the next two weeks")
            .WithOpenApi();

        movieEvents.MapPost("/", ScheduleMovieEventController.Invoke)
            .WithName("ScheduleMovieEvent")
            .WithDescription("Schedule a movie event")
            .Produces<object>(StatusCodes.Status201Created)
            .WithMetadata(new ConsumesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();

        movieEvents.MapGet("/movie", FindMovieByIdWithEventsController.Invoke)
            .WithName("FindMovieByIdWithEvents")
            .WithDescription("List all movie events for a specific movie")
            .WithOpenApi();

        movieEvents.MapGet("/monthly", FindMovieEventsForMonthController.Invoke)
            .WithName("FindMovieEventsForMonth")
            .WithDescription("List all movie events for a specific month")
            .WithOpenApi();

        movieEvents.MapPost("/{movieEventId}/bookings", BookMovieEventController.Invoke)
            .WithName("BookMovieEvent")
            .WithDescription("Book a movie event")
            .Produces<object>(StatusCodes.Status201Created)
            .WithMetadata(new ConsumesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();

        app.MapGet("/", ctx => {
            ctx.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });

        return app;
    }
}
