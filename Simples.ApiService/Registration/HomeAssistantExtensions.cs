using Simples.ApiService.Services.HomeAutomation;

namespace Simples.ApiService.Registration;

public static class HomeAssistantExtensions
{
    public static void AddHomeAssistant(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<GetFromHomeAutomationSerivce>();
        builder.Services.AddScoped<HomeAutomationWebSocket>();
        builder.Services.AddScoped<UpdateHomeAutomationService>();
        builder.Services.AddScoped<LightAutomationService>();

        // bearer token: HomeAssistant:LongLivedAccessToken
        builder.Services.AddHttpClient<HomeAssistantApiClient>((configure) => {
            
            var (host, token) = builder.Configuration.GetHomeAssistantConfig();

            configure.BaseAddress = new(host);
            configure.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        });
    }

    public static (string host, string token) GetHomeAssistantConfig(this IConfiguration configuration)
    {
        var host = configuration["HomeAssistant:Host"];
        var token = configuration["HomeAssistant:LongLivedAccessToken"];
        return (host, token);
    }
}
