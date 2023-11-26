namespace TcpServer;

public record Message
{
    public int Buid { get; set; }

    public string? Command { get; set; }
}