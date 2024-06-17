## IHttpClientFactory - Demo

This repo is an example of the best ways of using HttpClient and IHttpClientfactory.

Instantiating HttpClients directly in the components that will use it (not recommended):

```csharp

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

```

Instantiating HttpClients using factory, but it still in the components that will use it (not recommended):

```csharp

builder.Services.AddHttpClient();

// ...

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

```

Named HttpClients (recommended):

```csharp

// to add

```

Typed HttpClients (recommended):

```csharp

services.AddSingleton<IValidateOptions<OpenAiSettings>, OpenAiSettingsValidate>();
services.AddOptionsWithValidateOnStart<OpenAiSettings>()
    .Bind(configuration);

services.AddTransient<OpenAiGateway>();

services.AddHttpClient<OpenAiGateway>((serviceProvider, httpClient) =>
{
    var openAiSettings = serviceProvider.GetRequiredService<IOptions<OpenAiSettings>>();
    httpClient.BaseAddress = new (openAiSettings.Value.BaseAddress);
    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiSettings.Value.ApiKey}");
});

// ...

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

```

Messahe handler example:

```csharp

services.AddHttpClient<OpenAiGateway>((serviceProvider, httpClient) =>
{
    var openAiSettings = serviceProvider.GetRequiredService<IOptions<OpenAiSettings>>();
    httpClient.BaseAddress = new (openAiSettings.Value.BaseAddress);
    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiSettings.Value.ApiKey}");
})
    .AddHttpMessageHandler<LoggingHandler>();

// ...

public class LoggingHandler(ILogger<LoggingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Request: {Body}", await request.Content!.ReadAsStringAsync());

        var response = await base.SendAsync(request, cancellationToken);
        
        logger.LogInformation("StatusCode: {StatusCode}", response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        logger.LogInformation("Content: {Content}", responseContent);
        
        return response;
    }
}

```

This demo uses:
- .Net 9

---

| [<img src="https://github.com/wilsonneto-dev.png" width="75px;"/>][1] |
| :-: |
|[Wilson Neto][1]|


[1]: https://github.com/wilsonneto-dev
