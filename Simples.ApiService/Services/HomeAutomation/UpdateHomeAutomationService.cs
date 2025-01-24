using System.Text.Json;
using System.Text;
using System.Text.Unicode;

namespace Simples.ApiService.Services.HomeAutomation;

public sealed class UpdateHomeAutomationService(HomeAssistantApiClient homeAssistantApiClient)
{
    public async Task<string> UpdateStateAsync(string entityId, object newState, CancellationToken cancellationToken = default)
    {
        var route = HomeAutomationRoutes.States.UpdateState.Route(entityId);
        var content = new StringContent(JsonSerializer.Serialize(newState), Encoding.UTF8, "application/json");
        
        using var response = await homeAssistantApiClient.HttpClient.PostAsync(route, content, cancellationToken);
        
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}

public sealed class LightAutomationService(HomeAssistantApiClient homeAssistantApiClient) 
{
    public async Task<string> ChangeLightState(string entityId, OnOff state, CancellationToken cancellationToken = default)
    {
        var route = HomeAutomationRoutes.Service.Lights.Route(state);
        var json = $$"""
            {"entity_id": "{{entityId}}"}
            """;
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await homeAssistantApiClient.HttpClient.PostAsync(route, content, cancellationToken);

        response.EnsureSuccessStatusCode();

        var resultContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return resultContent;
    }
}
