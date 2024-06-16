using Microsoft.Extensions.Options;
using WebApplication1.OpenAi.Settings;

namespace WebApplication1.OpenAi;

internal class OpenAiGateway(IOptions<OpenAiSettings> openAiSettings)
{
    private readonly string _baseAddress = openAiSettings.Value.BaseAddress;
    private readonly string _apiKey = openAiSettings.Value.ApiKey;
    
    public Task<string> ExecutePrompt(string prompt) => 
        Task.FromResult($"BaseAddress: {_baseAddress} / Api Key: {_apiKey}");
}