
using Singulink.Enums;

namespace Simples.ApiService.Services.HomeAutomation;

/// <summary>
/// https://developers.home-assistant.io/docs/api/rest/
/// </summary>
public static partial class HomeAutomationRoutes
{
    public static class WebSocket
    {
        public static string Route() => "/api/websocket";
    }

    public static class Service 
    {
        public static class Lights   
        {
            public static string Route(OnOff state) => 
                $"/api/services/light/{state.AsRouteValue()}";
        }
    }

}