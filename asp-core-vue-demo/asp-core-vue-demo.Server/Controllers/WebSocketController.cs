using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace asp_core_vue_demo.Server.Controllers;

public class WebSocketController : ControllerBase
{
    [Route("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
           using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            // Run a parallel task to wait for events and then send them
           Task pushTask = Task.Run(() => RandomData(webSocket));

            await Echo(webSocket); // Wait for a close message, then return

            await pushTask; // Wait for any pending send operations to complete
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private static string GenerateName(int len)
    {
        Random r = new();
        string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
        string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
        var name = "";
        name += consonants[r.Next(consonants.Length)].ToUpper();
        name += vowels[r.Next(vowels.Length)];
        var b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
        while (b < len)
        {
            name += consonants[r.Next(consonants.Length)];
            b++;
            name += vowels[r.Next(vowels.Length)];
            b++;
        }

        return name;
    }

    private static async Task Echo(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!receiveResult.CloseStatus.HasValue)
        {
            await webSocket.SendAsync(
                new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                receiveResult.MessageType,
                receiveResult.EndOfMessage,
                CancellationToken.None);

            receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }

    private static async Task RandomData(WebSocket webSocket)
    {
        while (true)
        {
            var message = GenerateName(5);
            var bytes = Encoding.UTF8.GetBytes(message);
            var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);

            if (!webSocket.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(
                                arraySegment,
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None);

                await Task.Delay(2000);
            }
            else
            {
                break;
            }
        }
    }
}
