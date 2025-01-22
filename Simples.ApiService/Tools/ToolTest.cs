using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
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

    [KernelFunction("turn_on_all_lights")]
    [Description("Turn on all lights for all rooms")]
    public async Task<List<LightModel>> TurnOnAllLightsAsync(CancellationToken cancellationToken = default!)
    {
        return [];
    }

    [KernelFunction("Change_the_state_of_device")]
    [Description("Turn on all lights assigned to a room")]
    public async Task<List<LightModel>> TurnOnAllLightsForRoomAsync(string room, LightState targetState, CancellationToken cancellationToken = default!)
    {
        return [];
    }

    public enum LightState
    {
        Off = 0,
        On = 1,
    }
}

public sealed class LightModel
{
    public string Name { get; set; }
    public bool IsOn { get; set; }
    public int Brightness { get; set; }
    public string Hex { get; set; }
}

