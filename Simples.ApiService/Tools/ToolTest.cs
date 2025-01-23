using Microsoft.SemanticKernel;
using Simples.ApiService.Services.HomeAutomation;
using System.ComponentModel;

namespace Simples.ApiService.Tools;

public record QuickReference(string Id, string Name);

public class HomeAssistandPlugin(
    ILogger<HomeAssistandPlugin> logger,
    GetFromHomeAutomationSerivce getFromHomeAutomationSerivce, 
    UpdateHomeAutomationService updateHomeAutomationService)
{
    [KernelFunction("get_all_zones")]
    [Description("Gets the zones or rooms in the house")]
    public async Task<QuickReference[]> GetRoomsAsync(CancellationToken cancellationToken = default!)
    {
        logger.LogInformation("## Plugin: Getting zones");
        var results = await getFromHomeAutomationSerivce
            .GetAllStatesAsync(StatusType.Zone, cancellationToken);

        return results.Select(x=> new QuickReference(x.EntityId, x.FriendlyName)).ToArray();
    }

    [KernelFunction("get_all_lights")]
    [Description("Gets a list of devices like lights available in a room")]
    public async Task<QuickReference[]> GetAllLightDevicesAsync(CancellationToken cancellationToken = default!)
    {
        logger.LogInformation("## Plugin: Getting all lights");
        var results = await getFromHomeAutomationSerivce
            .GetAllStatesAsync(StatusType.Light, cancellationToken);

        return results.Select(x => new QuickReference(x.EntityId, x.FriendlyName)).ToArray();
    }

    [KernelFunction("get_all_lights_in_a_room")]
    [Description("Gets a list of devices like lights available in a room")]
    public async Task<ResultState[]> GetRoomDevicesAsync(string entityId, CancellationToken cancellationToken = default!)
    { 
        logger.LogInformation("## Plugin: Getting room devices");
        var results = await getFromHomeAutomationSerivce.GetSingleStateAsync(entityId, cancellationToken);
        return [];
    }

    [KernelFunction("turn_on_all_lights")]
    [Description("Turn on all lights for all rooms")]
    public async Task<ResultState[]> TurnOnAllLightsAsync(CancellationToken cancellationToken = default!)
    {
        logger.LogInformation("## Plugin: Turn on all lights");
        return [];
    }

    [KernelFunction("Change_the_state_of_device")]
    [Description("Turn on all lights assigned to a room")]
    public async Task<ResultState[]> TurnOnAllLightsForRoomAsync(string room, LightState targetState, CancellationToken cancellationToken = default!)
    {
        logger.LogInformation("## Plugin: Turn off all lights");
        return [];
    }

    public enum LightState
    {
        Off = 0,
        On = 1,
    }
}


