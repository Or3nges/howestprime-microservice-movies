using Howestprime.Movies.Main.Modules;
using Howestprime.Movies.Main.Modules.Messaging;
using Howestprime.Movies.Main.Modules.Persistence;
using Howestprime.Movies.Main.Modules.WebApi;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

/**
TODO: Only add the remaining modules if they are properly configured in the appsettings.json, and other settings files.
Otherwise, the app will not start.
**/
builder
    .Services
    .AddPersistenceModule(configuration)
    .AddWebApiModule(configuration)
    //    .AddMessagingModule(configuration)
    .AddUseCases();

var app = builder
    .Build()
    .UsePersistenceModule()
    .UseWebApiModule();

// await app.RunMessagingModule();
await app.RunAsync();
