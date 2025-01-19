namespace Simples.ApiService.Services.HomeAutomation;

public sealed class GetFromHomeAutomationSerivce(HomeAssistantApiClient homeAssistantApiClient)
{
    public async Task<string> GetAllStatesAsync(CancellationToken cancellationToken = default)
    {
        var route = HomeAutomationRoutes.States.GetAllStates.Route();
        using var response = await homeAssistantApiClient.HttpClient.GetAsync(route, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public async Task<string> GetSingleStateAsync(string entityId, CancellationToken cancellationToken = default)
    {
        var route = HomeAutomationRoutes.States.GetSingleDevice.Route(entityId);
        using var response = await homeAssistantApiClient.HttpClient.GetAsync(route, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
