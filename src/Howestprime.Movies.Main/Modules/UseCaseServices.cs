namespace Howestprime.Movies.Main.Modules;

using Howestprime.Movies.Application.Movies.FindMovieById;

public static class UseCaseServices
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<FindMovieByIdUseCase>();
        return services;
    }
}