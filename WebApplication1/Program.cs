using WebApplication1.ApiContracts;
using WebApplication1.OpenAi;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddOpenAi(configuration.GetSection("Providers:OpenAI"));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/", async (EndpointInput input, OpenAiGateway openAiGateway) =>
{
    var response = await openAiGateway.ExecutePrompt(input.Prompt);
    return Results.Ok(new EndpointOutput(response));
});

app.Run();