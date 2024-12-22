using Microsoft.Extensions.AI;

namespace Simples.ApiService.Api;

public static class ChatApiExtensions
{
    public static void MapChatApi(this WebApplication app)
    {
        app.MapGet("/chat", (IChatClient chat) => new ChatMessage("Hello, World!"))
            .WithName("GetChatMessage");

        app.MapPost("/chat", async (IChatClient chat, ChatMessage message) =>
        {
            var response = await chat.CompleteAsync(message.Message);
 
            return response;
        });
    }
    public record ChatMessage(string Message);
}
