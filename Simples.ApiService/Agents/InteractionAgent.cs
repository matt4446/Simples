using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.ChatCompletion;
using Simples.ApiService.Tools;

namespace Simples.ApiService.Agents;

/// <summary>
/// Generate automations 
/// </summary>
public sealed class CodeAgent
{
    public const string Name = nameof(CodeAgent);
    public const string AgentInstructions = """
        You create automations in YAML to control devices in HomeAssistant. 
        """;
}

public sealed class ChatAgent
{
    public const string Name = nameof(ChatAgent);
    public const string AgentInstructions = """
        You are a helpful agent but you dont do anyhthing with lights or devices. You may be giving work to the homeassistant agent.
        """;
}

/// <summary>
/// - Get devices from homeassistant
/// - Apply automations to homeassistant
/// </summary>
public sealed class HomeAssistantDeviceAgent 
{
    public const string Name = nameof(HomeAssistantDeviceAgent);
    public const string AgentInstructions = """
        You get devices from homeassistant and apply automations to homeassistant.
        You can also get the state of devices and control them individually. 
        You can reply in Markdown format where useful. 
        """;
}
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

public sealed class HomeAssistantAgents
{
    private readonly Kernel kernal;
    private readonly IChatCompletionService chatCompletionService;
    private readonly ChatHistory chatHistory;
    private readonly ChatCompletionAgent homeAutomationAgent;

    //private readonly PromptExecutionSettings promptSettings;
    private readonly ChatCompletionAgent chatAgent;
    private readonly AgentGroupChat chat;

    public HomeAssistantAgents([FromKeyedServices("HomeAssistantKernel")]Kernel kernel, HomeAssistandPlugin homeAssistandPlugin)
    {
        this.kernal = kernel;
        this.chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        this.chatHistory = new ChatHistory();
        this.chatHistory.AddSystemMessage(HomeAssistantDeviceAgent.AgentInstructions);
        // this.promptSettings = new PromptExecutionSettings()
        // {
        //     FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
        // };

        

        // chat 
        this.chatAgent = new ChatCompletionAgent(){
            Kernel = this.kernal,
            Name = ChatAgent.Name,  
            Instructions = ChatAgent.AgentInstructions
        };

        var controlHomeAssistantKernel = kernal.Clone();
        controlHomeAssistantKernel.Plugins.AddFromObject(homeAssistandPlugin);
        // do home automation things
        this.homeAutomationAgent = new ChatCompletionAgent()
        {
            Kernel = controlHomeAssistantKernel,
            Name = HomeAssistantDeviceAgent.Name,
            Instructions = HomeAssistantDeviceAgent.AgentInstructions,
            Arguments = new KernelArguments(new PromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            })
        };


        ChatHistoryTruncationReducer historyReducer = new(1);
        KernelFunction selectionFunction =
            AgentGroupChat.CreatePromptFunctionForStrategy(
                $$$"""
                Examine the provided RESPONSE and choose the next participant.
                State only the name of the chosen participant without explanation.
                Never choose the participant named in the RESPONSE.

                Choose only from these participants:
                - {{{ChatAgent.Name}}}
                - {{{HomeAssistantDeviceAgent.Name}}}

                Always follow these rules when choosing the next participant:
                - If RESPONSE is user input, it is {{{ChatAgent.Name}}}'s turn.
                - If RESPONSE is by {{{ChatAgent.Name}}}, it is {{{HomeAssistantDeviceAgent.Name}}}'s turn.
                - If RESPONSE is by {{{HomeAssistantDeviceAgent.Name}}}, it is {{{ChatAgent.Name}}}'s turn.

                RESPONSE:
                {{$lastmessage}}
                """,
                safeParameterNames: "lastmessage");

        string TerminationToken = "- Complete -";
        KernelFunction terminationFunction =
            AgentGroupChat.CreatePromptFunctionForStrategy(
                $$$"""
                Examine the RESPONSE and determine whether the content or action has been deemed satisfactory.
                If content is satisfactory, respond with a single word without explanation: {{{TerminationToken}}}.
                If specific suggestions are being provided, it is not satisfactory.
                If no correction is suggested, it is satisfactory.

                RESPONSE:
                {{$lastmessage}}
                """,
                safeParameterNames: "lastmessage");

        var agentGroupChat =
            new AgentGroupChat(chatAgent, homeAutomationAgent)
            {
                ExecutionSettings = new AgentGroupChatSettings
                {
                    SelectionStrategy =
                        new KernelFunctionSelectionStrategy(selectionFunction, kernel)
                        {
                            // Always start with the editor agent.
                            InitialAgent = chatAgent,
                            // Save tokens by only including the final response
                            HistoryReducer = historyReducer,
                            // The prompt variable name for the history argument.
                            HistoryVariableName = "lastmessage",
                            // Returns the entire result value as a string.
                            ResultParser = (result) => result.GetValue<string>() ?? ChatAgent.Name
                        },
                    TerminationStrategy =
                        new KernelFunctionTerminationStrategy(terminationFunction, kernel)
                        {
                            // Only evaluate for editor's response
                            Agents = [this.chatAgent, this.homeAutomationAgent],
                            // Save tokens by only including the final response
                            HistoryReducer = historyReducer,
                            // The prompt variable name for the history argument.
                            HistoryVariableName = "lastmessage",
                            // Limit total number of turns
                            MaximumIterations = 6,
                            // Customer result parser to determine if the response is "yes"
                            ResultParser = (result) => 
                                result.GetValue<string>()?.Contains(TerminationToken, StringComparison.OrdinalIgnoreCase) ?? false
                        }
                }
            };

       this.chat = agentGroupChat;

    }

    public async Task<ChatMessageContent[]> Chat(string message, CancellationToken ct)
    {
        await chat.ResetAsync();

        chat.AddChatMessage(new ChatMessageContent(AuthorRole.User, message));
        List<ChatMessageContent> chatMessageContents = new ();
        // Invoke agents
        await foreach (ChatMessageContent response in chat.InvokeAsync())
        {
            chatMessageContents.Add(response);
        // Process agent response(s)...
        }

        return chatMessageContents.ToArray();
    }

}
#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
