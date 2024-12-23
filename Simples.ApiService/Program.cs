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

builder.AddOllamaSharpChatClient("ollama-phi3-5");

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
