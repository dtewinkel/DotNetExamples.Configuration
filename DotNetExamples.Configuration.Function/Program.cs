using DotNetExamples.Configuration.Common;
using DotNetExamples.Configuration.Function;
using Microsoft.Extensions.Hosting;

new HostBuilder()
    .ConfigureFunctionsWebApplication()
    
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddProviders();
    })

    .ConfigureServices((context, services) =>
    {
        services.AddDemoConfiguration(context.Configuration);
    })

    .Build()
    .Run();
