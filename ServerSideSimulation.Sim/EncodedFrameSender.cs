using ServerSideSimulation.Lib.Channels;
using ServerSideSimulation.Lib.Encoding;
using ServerSideSimulation.Lib.Tcp;
using System.Net.Sockets;

namespace ServerSideSimulation.Sim
{
    /// <summary>
    /// Reads raw frames from a channel and outputs encoded frames to TCP.
    /// </summary>
    internal class EncodedFrameSender : ITcpHandler
    {
        private readonly BoundedChannel channel;
        private readonly FrameEncoder encoder;

        private const int secondsPerIFrame = 1;

        public EncodedFrameSender(RenderSettings settings)
        {
            encoder = new FrameEncoder(
                settings.ScreenWidth * settings.ScreenHeight,
                settings.Fps * secondsPerIFrame,
                new RunLengthEncoder()
            );
            channel = settings.Channel;
        }

        public async Task HandleClient(TcpClient client, CancellationToken cancellation)
        {
            using (var stream = client.GetStream())
            {
                await foreach (var frame in channel.ReadAllAsync())
                {
                    var encodedFrame = encoder.Encode(frame);
                    await stream.WriteAsync(encodedFrame.ToBytes(), cancellation);
                }
            }
        }
    }
}
