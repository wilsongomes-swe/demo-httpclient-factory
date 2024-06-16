using Microsoft.Extensions.Options;

namespace WebApplication1.OpenAi.Settings;

public class OpenAiSettingsValidate : IValidateOptions<OpenAiSettings>
{
    public ValidateOptionsResult Validate(string? name, OpenAiSettings options)
    {
        var errors = new List<string>(); 
        if(string.IsNullOrWhiteSpace(options.BaseAddress))
            errors.Add("BaseAddress not found");
        if(string.IsNullOrWhiteSpace(options.ApiKey))
            errors.Add("ApiKey not found");

        return errors.Count > 0 ? 
            ValidateOptionsResult.Fail(string.Join(';', errors)) 
            : ValidateOptionsResult.Success;
    }
}