using Microsoft.Extensions.AI;

namespace Simples.ApiService.Api;

public static class ChatApiExtensions
{
    public static void MapChatApi(this WebApplication app)
    {
        app.MapGet("/chat", (IChatClient chat) => new ChatMessage("Hello, World!"))
            .WithName("GetChatMessage");


        app.MapPost("/chat-simple", async (IChatClient chat, ChatMessage message) =>
        {
            var result = await chat.CompleteAsync(message.Message);

            return result;
        })
            .WithName("post-chat-simple");

        app.MapPost("/chat", async (IChatClient chat, ChatMessage message) =>
        {
            var stream = chat.CompleteStreamingAsync(message.Message);
 
            return stream;
        }).WithName("post-chat");
    }
    
}
public record ChatMessage(string Message);