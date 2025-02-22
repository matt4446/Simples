﻿using EnumsNET;

namespace Simples.ApiService.Services.HomeAutomation;

/// <summary>
/// https://developers.home-assistant.io/docs/api/rest/
/// </summary>
public static class HomeAutomationRoutes
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
                $"/api/services/light/{state.AsString(EnumFormat.EnumMemberValue)}";
        }
    }

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