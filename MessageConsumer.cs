using TcpServer;

using MassTransit;

public class MessageConsumer(Dictionary<int, IProtocol> connections) : IConsumer<Message>
{
    public async Task Consume(ConsumeContext<Message> context)
    {
        if (connections.TryGetValue(context.Message.Buid, out var protocol))
            await protocol.Command(context.Message.Command);
    }
}