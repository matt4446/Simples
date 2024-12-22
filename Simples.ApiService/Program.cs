using Microsoft.Extensions.AI;
using OpenAI.Chat;
using Simples.ApiService.Api;
using Simples.ApiService.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

//builder.AddOllamaClientApi("llama3");


// Learn more about configuring OpenAPI at https://aka.ms/awspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<OllamaApiClient>(client =>
{
    // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
    client.BaseAddress = new("phi3.5");
});
builder.Services.AddLogging(logging => {
    logging.SetMinimumLevel(LogLevel.Information);
    logging.AddConsole();
});

builder.AddOllamaSharpChatClient("ollama-phi3-5");

//builder.Services.AddSingleton<IChatClient>(static serviceProvider =>
//{
//    var logger = serviceProvider.GetRequiredService<ILogger<OllamaChatClient>>();
//    var config = serviceProvider.GetRequiredService<IConfiguration>();
//    var ollamaCnnString = config.GetConnectionString("ollama-phi3-5");
//    var defaultLLM = config["Aspire:OllamaSharp:ollama:Models:0"];

//    logger.LogInformation("Ollama connection string: {0}", ollamaCnnString);
//    logger.LogInformation("Default LLM: {0}", defaultLLM);

//    IChatClient chatClient = new OllamaChatClient(new Uri(ollamaCnnString), defaultLLM);

//    return chatClient;
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/ping", () => "Pong");
app.MapChatApi();
app.MapDefaultEndpoints();

app.Run();
