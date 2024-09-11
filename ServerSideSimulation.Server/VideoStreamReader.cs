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
            var host = "127.0.0.1";
            var port = 12345;
            Console.WriteLine($"Starting video stream TCP client at {host}:{port}");
            using (var client = new TcpClient())
            {
                await client.ConnectAsync(host, port);
                Console.WriteLine("Connected to video stream. Starting stream loop.");

                using (var stream = client.GetStream())
                {
                    // Calculate buffer size. Video should be compressed, but this is an upper bound based on the bitmap size.
                    var bitmapSize = 800 * 800 * 4;
                    var fragmentDurationSeconds = 1;
                    var fps = 30;
                    var framesPerFragment = fps * fragmentDurationSeconds;
                    var bufferSize = bitmapSize * framesPerFragment;

                    var inputBuffer = new byte[bufferSize];
                    while (!cancellation.IsCancellationRequested)
                    {
                        var size = await stream.ReadAsync(inputBuffer, cancellation);
                        var bytesRead = new byte[size];
                        Array.Copy(inputBuffer, bytesRead, bytesRead.Length);

                        if (size > 0)
                        {
                            Console.WriteLine($"VIDEO FRAME SIZE: {bytesRead.Length}");
                            channel.Write(bytesRead);
                        }
                    }
                }
            }

            Console.WriteLine("Ended video stream TCP listener.");
        }
    }
}
