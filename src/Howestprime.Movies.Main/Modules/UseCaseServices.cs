namespace Howestprime.Movies.Main.Modules;

using Howestprime.Movies.Application.Movies.FindMovieById;
using Howestprime.Movies.Application.Movies.ScheduleMovieEvent;
using Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents;

public static class UseCaseServices
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<FindMovieByIdUseCase>();
        services.AddScoped<ScheduleMovieEventUseCase>();
        services.AddScoped<FindMovieByIdWithEventsUseCase>();
        return services;
    }
}