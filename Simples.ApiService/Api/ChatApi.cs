using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.ChatCompletion;
using OllamaSharp.Models.Chat;
using Simples.ApiService.Agents;
using Simples.ApiService.Tools;

namespace Simples.ApiService.Api;

public static class ChatApiExtensions
{
    public static void MapChatApi(this WebApplication app)
    {
        app.MapGet("/ping", () => "Pong")
            .WithName("Pong");
        app.MapGet("/chat", ([FromServices] IChatClient chat) => new ChatMessage("Hello, World!"))
            .WithName("GetChatMessage");

        app.MapPost("/chat2", async ([FromServices] HomeAssistantAgent testChatContext, [FromBody] ChatMessage message, 
            CancellationToken cancellationToken) =>
        {
            var result = await testChatContext.Chat(message.Message, cancellationToken);
            return result;
        }).WithName("chat2");
    }
    
}
public record ChatMessage(string Message);