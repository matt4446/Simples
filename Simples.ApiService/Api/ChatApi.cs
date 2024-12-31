using Microsoft.Extensions.AI;
using Simples.ApiService.Tools;

namespace Simples.ApiService.Api;

public static class ChatApiExtensions
{
    public static void MapChatApi(this WebApplication app)
    {
        app.MapGet("/chat", (IChatClient chat) => new ChatMessage("Hello, World!"))
            .WithName("GetChatMessage");

        app.MapPost("/chat-simple-cat", async (IChatClient chat, TestChatContext testChatContext, ChatMessage message) =>
        {
            ChatOptions options = new()
            {
                Tools = new[] { AIFunctionFactory.Create(testChatContext.PetCatAsync) }
            };
            ChatCompletion result = await chat.CompleteAsync(message.Message, options);
            //https://github.com/dotnet/eShop/blob/633dd1a6525e705a85ed3f65e5167c2f901e51a7/src/WebApp/Components/Chatbot/ChatState.cs
            return result;
        }).WithName("chat-with-cat");


        app.MapPost("/chat-simple", async (IChatClient chat, TestChatContext testChatContext, ChatMessage message) =>
        {
            ChatCompletion result = await chat.CompleteAsync(message.Message);

            return result;
        }).WithName("post-chat-simple");

        app.MapPost("/chat", async (IChatClient chat, TestChatContext testChatContext, ChatMessage message) =>
        {
            IAsyncEnumerable<StreamingChatCompletionUpdate> stream = chat.CompleteStreamingAsync(message.Message);
 
            return stream;
        }).WithName("post-chat");
    }
    
}
public record ChatMessage(string Message);