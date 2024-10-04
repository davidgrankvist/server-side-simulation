using ServerSideSimulation.Lib.Channels;
using System.Net.Sockets;

namespace ServerSideSimulation.Lib.Tcp
{
    /// <summary>
    /// Reads from TCP and outputs to a channel.
    /// </summary>
    public class FrameReceiver : ITcpHandler
    {
        private readonly BoundedChannel channel;
        private readonly int bufferSize;

        public FrameReceiver(BoundedChannel channel, int bufferSize)
        {
            this.channel = channel;
            this.bufferSize = bufferSize;
        }

        public async Task HandleClient(TcpClient client, CancellationToken cancellation)
        {
            using (var stream = client.GetStream())
            {
                var inputBuffer = new byte[bufferSize];
                while (!cancellation.IsCancellationRequested)
                {
                    var size = await stream.ReadAsync(inputBuffer, cancellation);
                    if (size <= 0)
                    {
                        continue;
                    }

                    var outputBuffer = new byte[size];
                    Array.Copy(inputBuffer, outputBuffer, size);
                    channel.Write(outputBuffer);
                }
            }
        }
    }
}
