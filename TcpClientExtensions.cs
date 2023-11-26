namespace TcpServer;

using System.Net.Sockets;

public static class TcpClientExtensions
{
    public static async Task<byte[]> ReceiveAsync(
        this TcpClient client, byte[] buffer, CancellationToken cancellationToken)
    {
        try
        {
            return buffer.Take(await client.GetStream().ReadAsync(buffer, cancellationToken)).ToArray();
        }
        catch
        {
            return [];
        }
    }

    public static void RstAndDispose(this TcpClient client)
    {
        try
        {
            client.LingerState = new LingerOption(true, 0);
            client.Client.Close();
            client.Dispose();
        }
        catch
        {
        }
    }
}