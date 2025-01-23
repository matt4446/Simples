using System.Text.Json;
using System.Text.Json.Serialization;

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

        var all = models
            .Where(x => GetStateType(x) == stateType)   
            .Select(x => DeserializeDeviceState(x))
            .ToArray();


        return all;
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
    [JsonPropertyName("entity_id")]
    public string EntityId { get; set; }
    [JsonPropertyName("state")]
    public string State { get; set; }
    public string FriendlyName => Attributes?.FriendlyName;

    [JsonPropertyName("attributes")]
    public Attributes Attributes { get; set; }
    [JsonPropertyName("last_changed")]
    public DateTime LastChanged { get; set; }
    [JsonPropertyName("last_reported")]
    public DateTime LastReported { get; set; }
    [JsonPropertyName("last_updated")]
    public DateTime LastUpdated { get; set; }
}

public class PersonDevice : ResultState
{
    [JsonPropertyName("attributes")]
    public new PersonAttributes Attributes { get; set; }
}

public class ZoneDevice : ResultState
{
    [JsonPropertyName("attributes")]
    public new ZoneAttributes Attributes { get; set; }
}

public class SensorDevice : ResultState
{
    [JsonPropertyName("attributes")]
    public new SensorAttributes Attributes { get; set; }
}

public class LightDevice : ResultState
{
    [JsonPropertyName("attributes")]
    public new LightAttributes Attributes { get; set; }
}

public class Attributes
{
    [JsonPropertyName("editable")]
    public bool Editable { get; set; }
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }
    [JsonPropertyName("friendly_name")]
    public string FriendlyName { get; set; }
}

public class PersonAttributes : Attributes
{
    [JsonPropertyName("device_trackers")]
    public List<string> DeviceTrackers { get; set; }
}

public class ZoneAttributes : Attributes
{
    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }
    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
    [JsonPropertyName("radius")]
    public int? Radius { get; set; }
    [JsonPropertyName("passive")]
    public bool? Passive { get; set; }
    [JsonPropertyName("persons")]
    public List<string> Persons { get; set; }
    [JsonPropertyName("icon")]
    public string Icon { get; set; }
}

public class SensorAttributes : Attributes
{
    [JsonPropertyName("device_class")]
    public string DeviceClass { get; set; }
    [JsonPropertyName("next_dawn")]
    public DateTime? NextDawn { get; set; }
    [JsonPropertyName("next_dusk")]
    public DateTime? NextDusk { get; set; }
    [JsonPropertyName("next_midnight")]
    public DateTime? NextMidnight { get; set; }
    [JsonPropertyName("next_noon")]
    public DateTime? NextNoon { get; set; }
    [JsonPropertyName("next_rising")]
    public DateTime? NextRising { get; set; }
    [JsonPropertyName("next_setting")]
    public DateTime? NextSetting { get; set; }
    [JsonPropertyName("elevation")]
    public double? Elevation { get; set; }
    [JsonPropertyName("azimuth")]
    public double? Azimuth { get; set; }
    [JsonPropertyName("rising")]
    public bool? Rising { get; set; }
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }
    [JsonPropertyName("dew_point")]
    public double? DewPoint { get; set; }
    [JsonPropertyName("temperature_unit")]
    public string TemperatureUnit { get; set; }
    [JsonPropertyName("humidity")]
    public int? Humidity { get; set; }
    [JsonPropertyName("cloud_coverage")]
    public double? CloudCoverage { get; set; }
    [JsonPropertyName("uv_index")]
    public double? UvIndex { get; set; }
    [JsonPropertyName("pressure")]
    public double? Pressure { get; set; }
    [JsonPropertyName("pressure_unit")]
    public string PressureUnit { get; set; }
    [JsonPropertyName("wind_bearing")]
    public double? WindBearing { get; set; }
    [JsonPropertyName("wind_speed")]
    public double? WindSpeed { get; set; }
    [JsonPropertyName("wind_speed_unit")]
    public string WindSpeedUnit { get; set; }
    [JsonPropertyName("visibility_unit")]
    public string VisibilityUnit { get; set; }
    [JsonPropertyName("precipitation_unit")]
    public string PrecipitationUnit { get; set; }
    [JsonPropertyName("attribution")]
    public string Attribution { get; set; }
}

public class LightAttributes : Attributes
{
    [JsonPropertyName("min_color_temp_kelvin")]
    public int? MinColorTempKelvin { get; set; }
    [JsonPropertyName("max_color_temp_kelvin")]
    public int? MaxColorTempKelvin { get; set; }
    [JsonPropertyName("min_mireds")]
    public int? MinMireds { get; set; }
    [JsonPropertyName("max_mireds")]
    public int? MaxMireds { get; set; }
    [JsonPropertyName("effect_list")]
    public List<string> EffectList { get; set; }
    [JsonPropertyName("supported_color_modes")]
    public List<string> SupportedColorModes { get; set; }
    [JsonPropertyName("effect")]
    public string Effect { get; set; }
    [JsonPropertyName("color_mode")]
    public string ColorMode { get; set; }
    [JsonPropertyName("brightness")]
    public int? Brightness { get; set; }
    [JsonPropertyName("color_temp_kelvin")]
    public int? ColorTempKelvin { get; set; }
    [JsonPropertyName("color_temp")]
    public int? ColorTemp { get; set; }
    [JsonPropertyName("hs_color")]
    public List<double> HsColor { get; set; }
    [JsonPropertyName("rgb_color")]
    public List<int> RgbColor { get; set; }
    [JsonPropertyName("xy_color")]
    public List<double> XyColor { get; set; }
    [JsonPropertyName("mode")]
    public string Mode { get; set; }

    [JsonPropertyName("supported_features")]
    public int? SupportedFeatures { get; set; }
}
