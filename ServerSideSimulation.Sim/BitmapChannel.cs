using System.Threading.Channels;

namespace ServerSideSimulation.Sim
{
    internal class BitmapChannel
    {
        private readonly Channel<byte[]> channel;

        public BitmapChannel(int capacity)
        {
            channel = Channel.CreateBounded<byte[]>(new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
            });
        }

        public void Write(byte[] data)
        {
            channel.Writer.TryWrite(data);
        }

        public bool TryRead(out byte[] data)
        {
            data = [];

            if (channel.Reader.TryRead(out var item))
            {
                data = item;
                return true;
            }

            return false;
        }
    }
}
