using Howestprime.Movies.Infrastructure.Messaging.Shared.Contracts;
using Howestprime.Movies.Infrastructure.Messaging.Shared.Extensions;

namespace Howestprime.Movies.Main.Modules.Messaging;

public static class MessagingModule
{
    public static IServiceCollection AddMessagingModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return
            services
                .AddAmqpServices(configuration)
                .AddScoped<IControllerFactory, ControllerFactory>();
    }


    public static Task<IApplicationBuilder> RunMessagingModule(
        this WebApplication app
    )
    {
        return app.RunAmqpServices();
    }
}