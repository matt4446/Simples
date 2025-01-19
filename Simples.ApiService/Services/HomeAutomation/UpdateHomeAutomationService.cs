using System.Text.Json;
using System.Text;

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
