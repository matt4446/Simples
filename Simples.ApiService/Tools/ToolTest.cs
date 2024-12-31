using Microsoft.Extensions.AI;
using System.ComponentModel;

namespace Simples.ApiService.Tools;

public sealed class ChatContextBuilder(ToolBuilder toolBuilder)
{
    public ChatContext Build(ChatContextType chatContextType) 
    {
        return new ChatContext()
        {
            
        };
    }
}

public sealed class ChatContext()
{
    public ITool[] tools = [];
}

public sealed class TestChatContext(IChatClient chatClient) {
    public ChatMessage SystemMessage => new ChatMessage(ChatRole.System, "You are a cat. You only respond as a cat. Good cat.");
    public ChatMessage AssistantMessage => new ChatMessage(ChatRole.Assistant, "Meow. Im a cat");

    [Description("Pet Cat")]
    public async Task PetCatAsync(string userText) 
    {
        await chatClient.CompleteAsync(userText);
    }
}

public sealed class ToolBuilder(ILoggerFactory loggerFactory)
{
    private readonly List<ITool> allTools = new();


    private void Build() {
        ITool testTool = new TestTool(loggerFactory.CreateLogger<TestTool>());
        this.allTools.Add(testTool);

    }
}


public enum ChatContextType
{
    Test
}

public interface ITool 
{
    ChatContextType chatContextType { get; }
    public string Name { get; }
    public string Description { get; }

    public Task<string> Execute();
}

public sealed class TestTool(ILogger<TestTool> logger) : ITool
{
    public string Name => 
       "TestTool";

    public string Description =>
       "This is a test tool.";

    public ChatContextType chatContextType => ChatContextType.Test;
    public async Task<string> Execute()
    {
        logger.BeginScope("{ToolName}", Name);
        logger.LogInformation("Executing", Name);

        return "could do a thing";
    }
}