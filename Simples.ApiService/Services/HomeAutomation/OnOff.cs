using EnumsNET;
using System.Runtime.Serialization;

namespace Simples.ApiService.Services.HomeAutomation;

public enum OnOff
{
    [EnumMember(Value = "turn_on")]
    On,
    [EnumMember(Value = "turn_off")]
    Off
}
