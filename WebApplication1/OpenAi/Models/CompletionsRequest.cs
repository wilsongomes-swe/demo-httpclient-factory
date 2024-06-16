namespace WebApplication1.OpenAi.Models;

public record CompletionsRequest(string Model, Message[] Messages);