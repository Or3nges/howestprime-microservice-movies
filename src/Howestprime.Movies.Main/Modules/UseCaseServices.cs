namespace Howestprime.Movies.Main.Modules;

using Howestprime.Movies.Application.Movies.FindMovieById;
using Howestprime.Movies.Application.Movies.ScheduleMovieEvent;
using Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents;
using Howestprime.Movies.Application.Movies.FindMovieEventsForMonth;
using Howestprime.Movies.Application.Movies.FindMoviesWithEventsInTimeframe;

public static class UseCaseServices
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<FindMovieByIdUseCase>();
        services.AddScoped<ScheduleMovieEventUseCase>();
        services.AddScoped<FindMovieByIdWithEventsUseCase>();
        services.AddScoped<FindMovieEventsForMonthUseCase>();
        services.AddScoped<FindMoviesWithEventsInTimeframeUseCase>();
        return services;
    }
}