using System.Net.WebSockets;
using System.Text;

namespace Simples.ApiService.Services.HomeAutomation;

/// <summary>
/// todo: later
/// </summary>
public sealed class HomeAutomationWebSocket
{
    private readonly ClientWebSocket _webSocket = new ClientWebSocket();
    private readonly Uri uri;

    public HomeAutomationWebSocket(IConfiguration configuration) 
    {
        var url = configuration.GetValue<string>("Services:HomeAssistant:http")!;
        uri = new Uri(url);
    }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        await _webSocket.ConnectAsync(uri, cancellationToken);
    }

    public async Task SendAsync(string message, CancellationToken cancellationToken = default)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var segment = new ArraySegment<byte>(messageBytes);
        
        await _webSocket.SendAsync(segment, WebSocketMessageType.Text, true, cancellationToken);
    }

    public async Task<string> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        var buffer = new ArraySegment<byte>(new byte[1024]);
        var result = await _webSocket.ReceiveAsync(buffer, cancellationToken);
        
        return Encoding.UTF8.GetString(bytes: buffer.Array!, 0, result.Count);
    }

    public async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken = default)
    {
        await _webSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
    }
}
