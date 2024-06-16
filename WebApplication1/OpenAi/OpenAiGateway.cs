using Microsoft.Extensions.Options;
using WebApplication1.OpenAi.Models;
using WebApplication1.OpenAi.Settings;

namespace WebApplication1.OpenAi;

internal class OpenAiGateway(IOptions<OpenAiSettings> openAiSettings)
{
    private readonly string _baseAddress = openAiSettings.Value.BaseAddress;
    private readonly string _apiKey = openAiSettings.Value.ApiKey;
    
    public async Task<string> ExecutePrompt(string prompt)
    {
        var client = new HttpClient { BaseAddress = new (_baseAddress) };
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

        var httpResponse = await client.PostAsJsonAsync("chat/completions", 
            new CompletionsRequest("gpt-4o", [ new Message("user", prompt) ]));

        var response = await httpResponse.Content.ReadFromJsonAsync<CompletionsResponse>();
        
        return response!.Choices[0].Message.Content;
    }
}