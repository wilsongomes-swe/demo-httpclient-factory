using Microsoft.Extensions.Options;
using WebApplication1.OpenAi.Handlers;
using WebApplication1.OpenAi.Settings;

namespace WebApplication1.OpenAi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenAi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<LoggingHandler>();
        services.AddSingleton<IValidateOptions<OpenAiSettings>, OpenAiSettingsValidate>();
        services.AddOptionsWithValidateOnStart<OpenAiSettings>()
            .Bind(configuration);

        services.AddTransient<OpenAiGateway>();
        
        services.AddHttpClient<OpenAiGateway>((serviceProvider, httpClient) =>
        {
            var openAiSettings = serviceProvider.GetRequiredService<IOptions<OpenAiSettings>>();
            httpClient.BaseAddress = new (openAiSettings.Value.BaseAddress);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiSettings.Value.ApiKey}");
        })
        .AddHttpMessageHandler<LoggingHandler>();
        
        return services;
    }
}