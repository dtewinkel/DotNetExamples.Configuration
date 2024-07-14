using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace DotNetExamples.Configuration.Common;

public class NestedConfigData
{
    [Required]
    [Range(0, 100)]
    public int IntValue1 { get; set; }

    public int? IntValue2 { get; set; }
}

public abstract class ConfigData
{
    [Required]
    public required string FromSource { get; init; }

    [ValidateObjectMembers]
    public NestedConfigData? Nested { get; set; }

    public bool? IsSet { get; set; }

    [Url]
    public string? Url { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
}

public class ConfigDataSingleton : ConfigData;

public class ConfigDataScoped : ConfigData;

public class ConfigDataOptions : ConfigData;

public class ConfigDataOptionsSnapshot : ConfigData;

public class ConfigDataOptionsMonitor : ConfigData;

public class ConfigDataOptionsValidation : ConfigData
{
    [Required]
    public required string MustHave { get; init; }
}

public class ConfigDataOptionsValidationStartup : ConfigDataOptionsValidation;
