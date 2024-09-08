using System.Threading.Channels;

namespace ServerSideSimulation.Sim
{
    internal class BitmapChannel
    {
        private readonly int capacity;
        private Channel<byte[]> channel;

        public BitmapChannel(int capacity)
        {
            this.capacity = capacity;
        }

        public void Open()
        {
            channel = Channel.CreateBounded<byte[]>(new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
            });
        }

        public void Close()
        {
            channel.Writer.Complete();
        }

        public void Write(byte[] data)
        {
            channel.Writer.TryWrite(data);
        }

        public async IAsyncEnumerable<byte[]> ReadAllAsync()
        {
            while (await channel.Reader.WaitToReadAsync())
            {
                channel.Reader.TryRead(out var data);
                if (data != null)
                {
                    yield return data;
                }
            }
        }
    }
}
