namespace WebApplication1.OpenAi.Handlers;

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