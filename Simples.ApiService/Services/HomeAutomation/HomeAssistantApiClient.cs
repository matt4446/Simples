namespace Simples.ApiService.Services.HomeAutomation;

public sealed class HomeAssistantApiClient(HttpClient httpClient)
{
    public HttpClient HttpClient => httpClient;
}
