namespace Simples.ApiService.Clients;

public sealed class OllamaApiClient(HttpClient httpClient)
{
    public async Task<bool> Available() 
    {
        var result = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/health"));

        if (result.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }
}
