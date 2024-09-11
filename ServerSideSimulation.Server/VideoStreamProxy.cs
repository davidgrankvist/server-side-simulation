using System.Net;
using System.Net.Sockets;

namespace ServerSideSimulation.Server
{

    /// <summary>
    /// Output video stream to TCP for testing. See .\Scripts\start_reference_client_for_server.bat
    /// </summary>
    internal class VideoStreamProxy
    {
        private readonly VideoStreamChannel channel;

        public VideoStreamProxy(VideoStreamChannel channel)
        {
            this.channel = channel;
        }

        public async Task Run(CancellationToken cancellation)
        {
            var host = "127.0.0.1";
            var port = 12346;

            Console.WriteLine($"Starting TCP proxy listener at {host}:{port}");
            var listener = new TcpListener(IPAddress.Parse(host), port);
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
                await HandleClient(client, cancellation);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception while running proxy");
                Console.WriteLine(ex);
            }
        }

        private async Task HandleClient(TcpClient client, CancellationToken cancellation)
        {
            Console.WriteLine("Incoming TCP proxy connection. Starting send loop.");
            using (var stream = client.GetStream()) 
            {
                if (channel.HasDroppedInitialFrames)
                {
                    Console.WriteLine("The initial frames were dropped. Sending as separate messages");
                    foreach (var message in channel.InitialFrames)
                    {
                        Console.WriteLine($"Writing header of size {message.Length}");
                        await stream.WriteAsync(message, cancellation);
                    }
                }
                await foreach (var message in channel.ReadAllAsync())
                {
                    if (cancellation.IsCancellationRequested)
                    {
                        break;
                    }

                    await stream.WriteAsync(message, cancellation);
                }
            }
            Console.WriteLine("Ending proxy loop.");
        }
    }
}
