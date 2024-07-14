using DotNetExamples.Configuration.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Options;

namespace DotNetExamples.Configuration.Function;

public class GetConfigFunction
{
    private readonly ConfigDataSingleton _configDataSingleton;
    private readonly ConfigDataScoped _configDataScoped;
    private readonly IOptions<ConfigDataOptions> _options;
    private readonly IOptionsMonitor<ConfigDataOptionsMonitor> _optionsMonitor;
    private readonly IOptionsSnapshot<ConfigDataOptionsSnapshot> _optionsSnapshot;
    private readonly IOptionsMonitor<ConfigDataOptionsValidation> _optionsValidation;
    private readonly IOptionsMonitor<ConfigDataOptionsValidationStartup> _optionsValidationStartup;

    public GetConfigFunction(
        ConfigDataSingleton configDataSingleton, 
        ConfigDataScoped configDataScoped, 
        IOptions<ConfigDataOptions> options,
        IOptionsSnapshot<ConfigDataOptionsSnapshot> optionsSnapshot,
        IOptionsMonitor<ConfigDataOptionsMonitor> optionsMonitor,
        IOptionsMonitor<ConfigDataOptionsValidation> optionsValidation,
        IOptionsMonitor<ConfigDataOptionsValidationStartup> optionsValidationStartup
    )
    {
        _configDataSingleton = configDataSingleton;
        _configDataScoped = configDataScoped;
        _options = options;
        _optionsSnapshot = optionsSnapshot;
        _optionsMonitor = optionsMonitor;
        _optionsValidation = optionsValidation;
        _optionsValidationStartup = optionsValidationStartup;
    }

    [Function(nameof(AllConfigData))]
    public IActionResult AllConfigData([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        return new OkObjectResult(new  
        {
            When = DateTime.UtcNow.TimeOfDay,
            Singleton = _configDataSingleton, 
            Scoped = _configDataScoped,
            IOptions = _options.Value,
            IOptionsSnapshot = _optionsSnapshot.Value,
            IOptionsMonitor = _optionsMonitor.CurrentValue,
            IOptionsMonitorWithValidation = _optionsValidation,
            IOptionsMonitorWithValidationOnStartup = _optionsValidationStartup
        });
    }
}