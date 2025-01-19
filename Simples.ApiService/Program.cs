using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Ollama;
using OllamaSharp.Models;
using OpenAI.Chat;
using Simples.ApiService.Api;
using Simples.ApiService.Clients;
using Simples.ApiService.Services.HomeAutomation;
using Simples.ApiService.Tools;
#pragma warning disable SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

//builder.AddOllamaClientApi("llama3");
// Learn more about configuring OpenAPI at https://aka.ms/awspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<TestChatContext>();

builder.Services.AddHttpClient<OllamaApiClient>(client =>
{
    // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
    client.BaseAddress = new("phi4");
});

builder.Services.AddLogging(logging => {
    logging.SetMinimumLevel(LogLevel.Information);
    logging.AddConsole();
});

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy
                            .AllowAnyOrigin()
                            //.WithOrigins("http://localhost:5174")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                      });
});

// bearer token: HomeAssistant:LongLivedAccessToken
builder.Services.AddHttpClient<HomeAssistantApiClient>((configure) => {
    var token = builder.Configuration["HomeAssistant:LongLivedAccessToken"];
    configure.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
});

builder.Services.AddScoped<GetFromHomeAutomationSerivce>();
builder.Services.AddScoped<HomeAutomationWebSocket>();
builder.Services.AddScoped<UpdateHomeAutomationService>();

// doesnt support tools 
//builder.AddOllamaSharpChatClient("ollama-phi3-5");
//builder.AddOllamaSharpChatClient("phi4");

//builder.AddOllamaSharpChatClient("ollama-llama3-3");
builder.AddOllamaSharpChatClient("phi4", configure => { 
});
var settings = new OllamaPromptExecutionSettings { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };

var modelId = "phi4";
var endpoint = new Uri("phi4");

var kernalBuilder = Kernel.CreateBuilder();
kernalBuilder.AddInMemoryVectorStore();
kernalBuilder.AddOllamaChatCompletion(modelId: modelId, endpoint: endpoint, serviceId: null);


//builder.Services.AddOllamaChatCompletion(modelId, endpoint, null);

builder.AddOllamaSharpChatClient("phi4");

// https://github.com/microsoft/semantic-kernel/pull/9488/files#diff-1f00435eedeff3f558d3b83559e062b87516fed1e5ffbbbc689d8e38ea0e06f4
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
app.UseCors(MyAllowSpecificOrigins);

app.Run();
#pragma warning restore SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
