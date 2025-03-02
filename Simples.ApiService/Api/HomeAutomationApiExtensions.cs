using Simples.ApiService.Tools;

namespace Simples.ApiService.Api;

public static class HomeAutomationApiExtensions 
{ 
    public static void MapHomeAutomationApi(this WebApplication app)
    {
        app.MapGet("/home/status", (HomeAssistandPlugin homeAssistandPlugin) => homeAssistandPlugin.GetAllLightDevicesAsync())
            .WithName("home-status");
    }
}
