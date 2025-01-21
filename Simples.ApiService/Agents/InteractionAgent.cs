using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;

namespace Simples.ApiService.Agents;


/// <summary>
/// Make sense of hte input to delegate to other agents  
/// </summary>
public sealed class InteractionAgent 
{
    public const string AgentInstructions = """
        You help organsise automations 
        """;

    public AlternativeAgents OtherInteractiveClients = new AlternativeAgents();
}


public sealed class AlternativeAgents
{
    public InteractionAgent interactionAgent = new InteractionAgent();
}

/// <summary>
/// Generate automations 
/// </summary>
public sealed class CodeAgent
{
    public const string AgentInstructions = """
        You create automations in YAML to control devices in HomeAssistant. 
        """;


}

/// <summary>
/// - Get devices from homeassistant
/// - Apply automations to homeassistant
/// </summary>
public sealed class HomeAssistantDeviceAgent 
{ 
    public const string AgentInstructions = """
        You get devices from homeassistant and apply automations to homeassistant.
        You can also get the state of devices and control them individually
        """;



}


public sealed class HomeAssistantClient
{
    private readonly Kernel kernal;
    private readonly IChatCompletionService chatCompletionService;
    private readonly PromptExecutionSettings promptSettings;

    public HomeAssistantClient([FromKeyedServices("HomeAssistantKernal")] Kernel kernel)
    {
        this.kernal = kernel;
        this.chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        this.promptSettings = new PromptExecutionSettings()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
    }

    public async Task<ChatMessageContent> Chat(string message, CancellationToken ct)
    {
        ChatMessageContent chatResult = await chatCompletionService.GetChatMessageContentAsync(
            message, promptSettings, this.kernal, ct);

        return chatResult;
    }
}
