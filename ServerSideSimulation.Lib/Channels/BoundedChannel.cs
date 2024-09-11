using System.Threading.Channels;

namespace ServerSideSimulation.Lib.Channels
{
    public class BoundedChannel
    {
        private Channel<byte[]>? channel;
        private readonly BoundedChannelOptions options;

        public BoundedChannel(BoundedChannelOptions options)
        {
            this.options = options;
        }

        public static BoundedChannel CreateSingleReadWrite(int capacity)
        {
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleWriter = true,
                SingleReader = true,
            };
            return new BoundedChannel(options);
        }

        public static BoundedChannel CreateSingleWriteMultiRead(int capacity)
        {
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleWriter = true,
                SingleReader = false,
            };
            return new BoundedChannel(options);
        }

        public void Open()
        {
            channel = Channel.CreateBounded<byte[]>(options);
        }

        public void Close()
        {
            channel!.Writer.Complete();
        }

        public void Write(byte[] data)
        {
            channel!.Writer.TryWrite(data);
        }

        public async IAsyncEnumerable<byte[]> ReadAllAsync()
        {
            while (await channel!.Reader.WaitToReadAsync())
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
