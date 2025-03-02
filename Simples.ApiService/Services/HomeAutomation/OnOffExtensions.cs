namespace Simples.ApiService.Services.HomeAutomation;

public static class OnOffExtensions
{
    public static string AsRouteValue(this OnOff state) 
    {
        return state switch
        {
            OnOff.On => "turn_on",
            OnOff.Off => "turn_off",
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }
}