using DotNetExamples.Configuration.Common;
using Microsoft.Extensions.Hosting;

new HostBuilder()
    .ConfigureFunctionsWebApplication()
    
    .ConfigureAppConfiguration((context, builder) =>
    {
        // Add providers to the application. Specifying the base name of the settings files.
        builder.AddProviders<Program>("functionSettings", context.HostingEnvironment.EnvironmentName);
    })

    .ConfigureServices((context, services) =>
    {
        // Set up the configuration, based on the configuration providers defined earlier.
        services.AddDemoConfiguration(context.Configuration);
    })

    .Build()
    .Run();
