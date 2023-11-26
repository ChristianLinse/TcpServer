namespace TcpServer;

using System.Net.Sockets;

public interface IProtocol
{
    Task ExecuteAsync(TcpClient client, CancellationToken stoppingToken);
}
