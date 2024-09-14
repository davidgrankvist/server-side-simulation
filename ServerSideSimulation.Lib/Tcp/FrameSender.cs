using ServerSideSimulation.Lib.Channels;
using System.Net.Sockets;

namespace ServerSideSimulation.Lib.Tcp
{
    /// <summary>
    /// Reads from a channel and outputs to TCP.
    /// </summary>
    public class FrameSender : ITcpHandler
    {
        private readonly BoundedChannel channel;

        public FrameSender(BoundedChannel channel)
        {
            this.channel = channel;
        }

        public async Task HandleClient(TcpClient client, CancellationToken cancellation)
        {
            using (var stream = client.GetStream())
            {
                await foreach (var frame in channel.ReadAllAsync())
                {
                    await stream.WriteAsync(frame, cancellation);
                }
            }
        }
    }
}
