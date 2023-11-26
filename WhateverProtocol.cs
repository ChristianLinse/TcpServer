namespace TcpServer;

using System.Net.Sockets;
using System.Text;
using MassTransit;

public class WhateverProtocol(
    ILogger<WhateverProtocol> logger, Dictionary<int, IProtocol> connections, IBus bus)
    : IProtocol
{
    private readonly byte[] buffer = new byte[1024];
    private TcpClient client;

    public async Task ExecuteAsync(TcpClient client, CancellationToken stoppingToken)
    {
        this.client = client;
        connections.TryAdd(GetHashCode(), this);

        while (stoppingToken.IsCancellationRequested is false)
        {
            using var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                stoppingToken, new CancellationTokenSource(TimeSpan.FromMinutes(10)).Token);
            var segment = await client.ReceiveAsync(buffer, cancellationTokenSource.Token);

            if (segment.Length > 0)
            {
                // Do stuff
                logger.LogInformation(
                    "{hash_code} {message}", GetHashCode(), Encoding.ASCII.GetString(segment));
                await client.SendAsync(Encoding.ASCII.GetBytes(
                    "test" + Environment.NewLine), cancellationTokenSource.Token);
                await bus.Publish(new Message()
                {
                    Buid = GetHashCode(),
                    Command = "rabbit",
                }, cancellationTokenSource.Token);
                // Do more stuff
            }
            else
            {
                // Cleanup stuff
                connections.Remove(GetHashCode());
                client.RstAndDispose();
                return;
            }
        }
    }

    public async Task Command(string message)
    {
        await client.SendAsync(Encoding.ASCII.GetBytes(message + Environment.NewLine), default);
    }
}
