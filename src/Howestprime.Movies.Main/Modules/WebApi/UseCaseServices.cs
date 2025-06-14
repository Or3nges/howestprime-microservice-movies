namespace Howestprime.Movies.Main.Modules;

using Howestprime.Movies.Application;
using Howestprime.Movies.Application.Movies.FindMovieById;
using Howestprime.Movies.Application.Movies.ScheduleMovieEvent;
using Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents;
using Howestprime.Movies.Application.Movies.FindMovieEventsForMonth;
using Howestprime.Movies.Application.Movies.FindMoviesWithEventsInTimeframe;
using Howestprime.Movies.Application.Movies.RegisterMovie;
using Howestprime.Movies.Application.Movies.FindMoviesByFilters;
using Howestprime.Movies.Application.Movies.BookMovieEvent;
using Howestprime.Movies.Infrastructure.Messaging.Shared;
using Howestprime.Movies.Infrastructure.Messaging.Publishers;
using Howestprime.Movies.Application.Contracts.Ports;

public static class UseCaseServices
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<RegisterMovieUseCase>();
        services.AddScoped<FindMoviesByFiltersUseCase>();
        services.AddScoped<FindMovieByIdUseCase>();
        services.AddScoped<FindMovieByIdWithEventsUseCase>();
        services.AddScoped<ScheduleMovieEventUseCase>();
        services.AddScoped<FindMovieEventsForMonthUseCase>();
        services.AddScoped<FindMoviesWithEventsInTimeframeUseCase>();
        services.AddScoped<BookMovieEventHandler>(sp => new BookMovieEventHandler(
            sp.GetRequiredService<IMovieEventRepository>(),
            sp.GetRequiredService<IUnitOfWork>(),
            sp.GetRequiredService<IEventPublisher>(),
            sp.GetRequiredService<IRoomRepository>()
        ));
        services.AddScoped<IEventPublisher, MovieEventPublisher>();
        return services;
    }
}