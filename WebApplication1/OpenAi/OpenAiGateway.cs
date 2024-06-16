using Microsoft.Extensions.Options;
using WebApplication1.OpenAi.Models;
using WebApplication1.OpenAi.Settings;

namespace WebApplication1.OpenAi;

internal class OpenAiGateway
{
    private readonly HttpClient _httpClient;

    public OpenAiGateway(IOptions<OpenAiSettings> openAiSettings, IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new (openAiSettings.Value.BaseAddress);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiSettings.Value.ApiKey}");
    }

    public async Task<string> ExecutePrompt(string prompt)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync("chat/completions", 
            new CompletionsRequest("gpt-4o", [ new Message("user", prompt) ]));

        var response = await httpResponse.Content.ReadFromJsonAsync<CompletionsResponse>();
        
        return response!.Choices[0].Message.Content;
    }
}