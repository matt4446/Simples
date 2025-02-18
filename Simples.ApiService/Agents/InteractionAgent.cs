using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Simples.ApiService.Agents;

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
        You can also get the state of devices and control them individually. 
        You can reply in Markdown format where useful. 
        """;
}

public sealed class HomeAssistantAgent
{
    private readonly Kernel kernal;
    private readonly IChatCompletionService chatCompletionService;
    private readonly ChatHistory chatHistory;
    private readonly PromptExecutionSettings promptSettings;

    public HomeAssistantAgent([FromKeyedServices("HomeAssistantKernel")]Kernel kernel)
    {
        this.kernal = kernel;
        this.chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        this.chatHistory = new ChatHistory();
        this.chatHistory.AddSystemMessage(HomeAssistantDeviceAgent.AgentInstructions);
        this.promptSettings = new PromptExecutionSettings()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
        };
    }
#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    public async Task<ChatMessageContent> Chat(string message, CancellationToken ct)
    {
        chatHistory.AddUserMessage(message);

        //ChatCompletionAgent agent =
        //    new()
        //    {
        //        Name = "SummarizationAgent",
        //        Instructions = "Summarize user input",
        //        Kernel = this.kernal
        //    };

        //agent.InvokeAsync(chatHistory, promptSettings, ct);

        ChatMessageContent chatResult = await chatCompletionService.GetChatMessageContentAsync(
            chatHistory, promptSettings, kernal, ct);

        return chatResult;
    }
#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

}
