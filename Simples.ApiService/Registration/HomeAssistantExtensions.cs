using Simples.ApiService.Services.HomeAutomation;

namespace Simples.ApiService.Registration;

public static class HomeAssistantExtensions
{
    public static void AddHomeAssistant(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<GetFromHomeAutomationSerivce>();
        builder.Services.AddScoped<HomeAutomationWebSocket>();
        builder.Services.AddScoped<UpdateHomeAutomationService>();


        builder.Services.AddHttpClient<HomeAssistantApiClient>((configure) => {
            var token = builder.Configuration["HomeAssistant:LongLivedAccessToken"];
            
            configure.BaseAddress = new("http://homeassistant");
            configure.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        });
    }
}
