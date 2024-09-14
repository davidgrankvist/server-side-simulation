using System.Net;
using System.Net.Sockets;

namespace ServerSideSimulation.Lib.Tcp
{
    public class TcpServer
    {
        private readonly IPEndPoint endPoint;
        private readonly ITcpHandler handler;

        public TcpServer(IPEndPoint endPoint, ITcpHandler handler)
        {
            this.endPoint = endPoint;
            this.handler = handler;
        }

        public async Task Run(CancellationToken cancellation)
        {
            Console.WriteLine($"Starting TCP listener at {endPoint.Address}:{endPoint.Port}");
            var listener = new TcpListener(endPoint.Address, endPoint.Port);
            listener.Start();

            while (!cancellation.IsCancellationRequested)
            {
                TcpClient client = listener.AcceptTcpClient();
                await HandleClientWithError(client, cancellation);
            }

            listener.Stop();
        }

        private async Task HandleClientWithError(TcpClient client, CancellationToken cancellation)
        {
            try
            {
                await handler.HandleClient(client, cancellation);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception while running TCP server");
                Console.WriteLine(ex);
            }
        }
    }
}
