namespace TcpServer;

using System.Net.Sockets;
using System.Text;

public class WhateverProtocol(ILogger<WhateverProtocol> logger) : IProtocol
{
    private readonly byte[] buffer = new byte[1024];

    public async Task ExecuteAsync(TcpClient client, CancellationToken stoppingToken)
    {
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
                // Do more stuff
            }
            else
            {
                // Cleanup stuff
                client.RstAndDispose();
                return;
            }
        }
    }
}
