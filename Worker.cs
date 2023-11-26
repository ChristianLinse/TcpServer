namespace TcpServer;

using System.Net;
using System.Net.Sockets;

public class Worker(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    private readonly TcpListener listener = new TcpListener(new IPAddress(0), 12345);

    public override void Dispose()
    {
        listener.Dispose();
        base.Dispose();
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        listener.Start();
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            await listener.AcceptTcpClientAsync(stoppingToken).AsTask().ContinueWith(async task =>
            {
                using var scope = serviceScopeFactory.CreateScope();
                await scope.ServiceProvider.GetRequiredService<IProtocol>().ExecuteAsync(
                    task.Result, stoppingToken);
            });
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        listener.Stop();
        await base.StopAsync(cancellationToken);
    }
}
