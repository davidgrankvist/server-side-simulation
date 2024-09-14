using System.Net.Sockets;

namespace ServerSideSimulation.Lib.Tcp
{
    public interface ITcpHandler
    {
        Task HandleClient(TcpClient client, CancellationToken cancellation);
    }
}
