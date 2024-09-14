using System.Net;
using System.Net.Sockets;

namespace ServerSideSimulation.Lib.Tcp
{
    public class TcpClientWrapper
    {
        private readonly IPEndPoint endPoint;
        private readonly ITcpHandler handler;

        public TcpClientWrapper(IPEndPoint endPoint, ITcpHandler handler)
        {
            this.endPoint = endPoint;
            this.handler = handler;
        }

        public async Task Run(CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                await HandleClientWithError(cancellation);
            }
        }

        private async Task HandleClientWithError(CancellationToken cancellation)
        {
            try
            {
                await StartClient(cancellation);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught error when running TCP client", ex);
                Console.WriteLine(ex);
            }
        }

        private async Task StartClient(CancellationToken cancellation)
        {
            Console.WriteLine($"Starting TCP client at {endPoint.Address}:{endPoint.Port}");
            using (var client = new TcpClient())
            {
                await client.ConnectAsync(endPoint.Address, endPoint.Port);
                await handler.HandleClient(client, cancellation);
            }
        }
    }
}
