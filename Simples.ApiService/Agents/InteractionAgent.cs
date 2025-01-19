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
