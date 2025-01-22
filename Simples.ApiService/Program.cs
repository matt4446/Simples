using Simples.ApiService.Api;
using Simples.ApiService.Registration;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/awspnet/openapi
builder.Services.AddOpenApi();

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

builder.AddHomeAssistant();
builder.AddLocalChatClients();

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
