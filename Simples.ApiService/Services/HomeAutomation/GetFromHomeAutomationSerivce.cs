using System.Text.Json;

namespace Simples.ApiService.Services.HomeAutomation;

public sealed class GetFromHomeAutomationSerivce(HomeAssistantApiClient homeAssistantApiClient)
{
    public async Task<ResultState[]> GetAllStatesAsync(StatusType stateType, CancellationToken cancellationToken = default)
    {
        var route = HomeAutomationRoutes.States.GetAllStates.Route();
        using var response = await homeAssistantApiClient.HttpClient.GetAsync(route, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        var models = JsonSerializer.Deserialize<JsonElement[]>(jsonResponse);

        return models
            .Where(x => GetStateType(x) == stateType)   
            .Select(x => DeserializeDeviceState(x))
            .ToArray();
    }

    public async Task<ResultState> GetSingleStateAsync(string entityId, CancellationToken cancellationToken = default)
    {
        var route = HomeAutomationRoutes.States.GetSingleDevice.Route(entityId);
        using var response = await homeAssistantApiClient.HttpClient.GetAsync(route, cancellationToken);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        var models = JsonSerializer.Deserialize<JsonElement[]>(jsonResponse);

        return null;
    }

    private static StatusType GetStateType(JsonElement jsonElement) 
    {
        var entityId = jsonElement.GetProperty("entity_id").GetString();
        return entityId switch
        {
            var _ when entityId.StartsWith("person", StringComparison.OrdinalIgnoreCase) => StatusType.Person,
            var _ when entityId.StartsWith("zone", StringComparison.OrdinalIgnoreCase) => StatusType.Zone,
            var _ when entityId.StartsWith("sensor", StringComparison.OrdinalIgnoreCase) => StatusType.Sensor,
            var _ when entityId.StartsWith("light", StringComparison.OrdinalIgnoreCase) => StatusType.Light,
            _ => StatusType.Unknown
        };

    }
    private static ResultState DeserializeDeviceState(JsonElement jsonElement)
    {
        var entityId = jsonElement.GetProperty("entity_id").GetString();
        if (entityId.StartsWith("person", StringComparison.OrdinalIgnoreCase)) 
        {
            return JsonSerializer.Deserialize<PersonDevice>(jsonElement.GetRawText());
        }
        if (entityId.StartsWith("zone", StringComparison.OrdinalIgnoreCase))
        {
            return JsonSerializer.Deserialize<ZoneDevice>(jsonElement.GetRawText());
        }
        if (entityId.StartsWith("sensor", StringComparison.OrdinalIgnoreCase))
        {
            return JsonSerializer.Deserialize<SensorDevice>(jsonElement.GetRawText());
        }
        if (entityId.StartsWith("light", StringComparison.OrdinalIgnoreCase))
        {
            return JsonSerializer.Deserialize<LightDevice>(jsonElement.GetRawText());
        }

        return null;
    }
}




public enum StatusType 
{
    Unknown,
    Person,
    Zone,
    Sensor,
    Light,
}

public class ResultState
{
    public string EntityId { get; set; }
    public string State { get; set; }
    public string FriendlyName => Attributes?.FriendlyName ?? EntityId.Split(".")[1];
    
    public Attributes Attributes { get; set; }
    public DateTime LastChanged { get; set; }
    public DateTime LastReported { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class PersonDevice : ResultState
{
    public new PersonAttributes Attributes { get; set; }
}

public class ZoneDevice : ResultState
{
    public new ZoneAttributes Attributes { get; set; }
}

public class SensorDevice : ResultState
{
    public new SensorAttributes Attributes { get; set; }
}

public class LightDevice : ResultState
{
    public new LightAttributes Attributes { get; set; }
}

public class Attributes
{
    public bool Editable { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public string FriendlyName { get; set; }
}

public class PersonAttributes : Attributes
{
    public List<string> DeviceTrackers { get; set; }
}

public class ZoneAttributes : Attributes
{
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? Radius { get; set; }
    public bool? Passive { get; set; }
    public List<string> Persons { get; set; }
    public string Icon { get; set; }
}

public class SensorAttributes : Attributes
{
    public string DeviceClass { get; set; }
    public DateTime? NextDawn { get; set; }
    public DateTime? NextDusk { get; set; }
    public DateTime? NextMidnight { get; set; }
    public DateTime? NextNoon { get; set; }
    public DateTime? NextRising { get; set; }
    public DateTime? NextSetting { get; set; }
    public double? Elevation { get; set; }
    public double? Azimuth { get; set; }
    public bool? Rising { get; set; }
    public double? Temperature { get; set; }
    public double? DewPoint { get; set; }
    public string TemperatureUnit { get; set; }
    public int? Humidity { get; set; }
    public double? CloudCoverage { get; set; }
    public double? UvIndex { get; set; }
    public double? Pressure { get; set; }
    public string PressureUnit { get; set; }
    public double? WindBearing { get; set; }
    public double? WindSpeed { get; set; }
    public string WindSpeedUnit { get; set; }
    public string VisibilityUnit { get; set; }
    public string PrecipitationUnit { get; set; }
    public string Attribution { get; set; }
}

public class LightAttributes : Attributes
{
    public int? MinColorTempKelvin { get; set; }
    public int? MaxColorTempKelvin { get; set; }
    public int? MinMireds { get; set; }
    public int? MaxMireds { get; set; }
    public List<string> EffectList { get; set; }
    public List<string> SupportedColorModes { get; set; }
    public string Effect { get; set; }
    public string ColorMode { get; set; }
    public int? Brightness { get; set; }
    public int? ColorTempKelvin { get; set; }
    public int? ColorTemp { get; set; }
    public List<double> HsColor { get; set; }
    public List<int> RgbColor { get; set; }
    public List<double> XyColor { get; set; }
    public string Mode { get; set; }
    public string Dynamics { get; set; }
    public bool? IsHueGroup { get; set; }
    public List<string> HueScenes { get; set; }
    public string HueType { get; set; }
    public List<string> Lights { get; set; }
    public List<string> EntityId { get; set; }
}