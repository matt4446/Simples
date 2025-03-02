
namespace Simples.ApiService.Services.HomeAutomation;

public static partial class HomeAutomationRoutes
{
    public static class States
    {
        public static class GetAllStates
        {
            public static string Route() => "/api/states";
        }
        public static class GetSingleDevice
        {
            public static string Route(string entityId) => $"/api/states/{entityId}";
        }

        public static class UpdateState 
        {
            public static string Route(string entityId) => $"/api/states/{entityId}";
        }
    }

    

}