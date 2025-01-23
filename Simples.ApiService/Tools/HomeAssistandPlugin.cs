using Microsoft.SemanticKernel;
using Simples.ApiService.Services.HomeAutomation;
using System.ComponentModel;

namespace Simples.ApiService.Tools;

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

    //[KernelFunction("get_lights_belonging_to_a_zone")]
    //[Description("Gets a list of devices like lights available in a room belonging to the entity id")]
    //public async Task<ResultState[]> GetRoomDevicesAsync(string entityId, CancellationToken cancellationToken = default!)
    //{ 
    //    logger.LogInformation("## Plugin: Getting room devices");
    //    var results = await getFromHomeAutomationSerivce.GetSingleStateAsync(entityId, cancellationToken);
    //    return [];
    //}

    [KernelFunction("turn_off_all_lights")]
    [Description("Turn off all lights")]
    public async Task<bool> TurnOffAllLightsAsync(CancellationToken cancellationToken = default!)
    {
        logger.LogInformation("## Plugin: Turn off all lights");
        var lights = await this.GetAllLightDevicesAsync(cancellationToken);

        var allOff = lights.Select(device => updateHomeAutomationService.UpdateStateAsync(device.Id, new { 
            state = "off"
        })).ToArray();

        await Task.WhenAll(allOff);

        return true;
    }

    [KernelFunction("turn_on_all_lights")]
    [Description("Turn on all lights")]
    public async Task<bool> TurnOnAllLightsAsync(CancellationToken cancellationToken = default!)
    {
        logger.LogInformation("## Plugin: Turn on all lights");
        var lights = await this.GetAllLightDevicesAsync(cancellationToken);

        var allOn = lights.Select(device => updateHomeAutomationService.UpdateStateAsync(device.Id, new
        {
            state = "on"
        })).ToArray();

        await Task.WhenAll(allOn);

        return true;
    }

    public enum LightState
    {
        Off = 0,
        On = 1,
    }
}


