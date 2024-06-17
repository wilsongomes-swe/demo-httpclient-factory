using Microsoft.Extensions.Options;
using WebApplication1.OpenAi.Models;
using WebApplication1.OpenAi.Settings;

namespace WebApplication1.OpenAi;

internal class OpenAiGateway(HttpClient httpClient)
{
    public async Task<string> ExecutePrompt(string prompt)
    {
        var httpResponse = await httpClient.PostAsJsonAsync("chat/completions", 
            new CompletionsRequest("gpt-4o", [ new Message("user", prompt) ]));

        var response = await httpResponse.Content.ReadFromJsonAsync<CompletionsResponse>();
        
        return response!.Choices[0].Message.Content;
    }
}