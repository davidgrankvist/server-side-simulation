using System.Threading.Channels;

namespace ServerSideSimulation.Server
{
    // TODO(improvement): reuse sim channel code
    internal class VideoStreamChannel
    {
        private Channel<byte[]> channel;
        private readonly int capacity;

        public VideoStreamChannel(int capacity)
        {
            this.capacity = capacity;
        }

        public void Open()
        {
            channel = Channel.CreateBounded<byte[]>(new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleWriter = true,
                SingleReader = false,
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
