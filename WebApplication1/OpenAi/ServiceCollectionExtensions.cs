using Microsoft.Extensions.Options;
using WebApplication1.OpenAi.Settings;

namespace WebApplication1.OpenAi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenAi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidateOptions<OpenAiSettings>, OpenAiSettingsValidate>();
        services.AddOptionsWithValidateOnStart<OpenAiSettings>()
            .Bind(configuration);

        services.AddTransient<OpenAiGateway>();
        
        return services;
    }
}