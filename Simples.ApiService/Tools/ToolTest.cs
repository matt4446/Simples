using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Simples.ApiService.Services.HomeAutomation;
using System.ComponentModel;

namespace Simples.ApiService.Tools;

public class HomeAssistandPlugin(
    GetFromHomeAutomationSerivce getFromHomeAutomationSerivce, 
    UpdateHomeAutomationService updateHomeAutomationService)
{
    [KernelFunction("get_rooms")]
    [Description("Gets a list rooms that can be controlled")]
    public async Task<List<LightModel>> GetRoomsAsync(CancellationToken cancellationToken = default!)
    {
        var results = await getFromHomeAutomationSerivce.GetAllStatesAsync(cancellationToken);
        return [];
    }

    [KernelFunction("get_room_devices")]
    [Description("Gets a list of devices like lights available int the room")]
    public async Task<List<LightModel>> GetRoomDevicesAsync(string entityId, CancellationToken cancellationToken = default!)
    {
        var results = await getFromHomeAutomationSerivce.GetSingleStateAsync(entityId, cancellationToken);
        return [];
    }

    [KernelFunction("turn on a all lights")]
    [Description("Turn on all lights for all rooms")]
    public async Task<List<LightModel>> TurnOnAllLightsAsync(CancellationToken cancellationToken = default!)
    {
        return [];
    }

    [KernelFunction("Change the state of a room's lights")]
    [Description("Turn on all lights assigned to a room")]
    public async Task<List<LightModel>> TurnOnAllLightsForRoomAsync(string room, LightState targetState, CancellationToken cancellationToken = default!)
    {
        return [];
    }

    public enum LightState
    {
        On,
        Off
    }
}

public sealed class LightModel
{
    public string Name { get; set; }
    public bool IsOn { get; set; }
    public int Brightness { get; set; }
    public string Hex { get; set; }
}


public sealed class ChatContextBuilder(ToolBuilder toolBuilder)
{
    public ChatContext Build(ChatContextType chatContextType) 
    {
        return new ChatContext()
        {
            
        };
    }
}

public sealed class Weather 
{ 
    
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
        logger.LogInformation("Executing {ToolName}", Name);

        return "could do a thing";
    }
}