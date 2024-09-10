using System.Net.Sockets;

namespace ServerSideSimulation.Server
{
    internal class VideoStreamReader
    {
        private readonly VideoStreamChannel channel;
        
        public VideoStreamReader(VideoStreamChannel channel)
        {
            this.channel = channel;
        }

        public Task Run(CancellationToken cancellation)
        {
            return StartVideoStreamListenerAndCheckErrors(cancellation);
        }

        private async Task StartVideoStreamListenerAndCheckErrors(CancellationToken cancellation)
        {
            try
            {
                await StartVideoStreamListener(cancellation);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught error when listening to video stream", ex);
                Console.WriteLine(ex);
            }
        }

        private async Task StartVideoStreamListener(CancellationToken cancellation)
        {
            Console.WriteLine("Starting video stream TCP listener.");
            using (var client = new TcpClient())
            {
                await client.ConnectAsync("127.0.0.1", 12345);
                Console.WriteLine("Connected to video stream. Starting stream loop.");

                using (var stream = client.GetStream())
                {
                    var buffer = new byte[1024];
                    while (!cancellation.IsCancellationRequested)
                    {
                        var size = await stream.ReadAsync(buffer, cancellation);
                        var bytesRead = new byte[size];
                        Array.Copy(buffer, bytesRead, bytesRead.Length);

                        channel.Write(bytesRead);
                    }
                }
            }

            Console.WriteLine("Ended video stream TCP listener.");
        }
    }
}
