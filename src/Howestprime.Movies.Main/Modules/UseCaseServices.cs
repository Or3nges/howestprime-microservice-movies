namespace Howestprime.Movies.Main.Modules;

using Howestprime.Movies.Application.Movies.FindMovieById;
using Howestprime.Movies.Application.Movies.ScheduleMovieEvent;

public static class UseCaseServices
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<FindMovieByIdUseCase>();
        services.AddScoped<ScheduleMovieEventUseCase>();
        return services;
    }
}