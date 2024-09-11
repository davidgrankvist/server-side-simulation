using ServerSideSimulation.Lib.Channels;

namespace ServerSideSimulation.Server
{
    internal class VideoStreamChannel
    {
        private readonly BoundedChannel channel;
        private readonly int capacity;

        private int writeCount;
        private List<byte[]> initialFrames;
        private int numInitialFrames = 2;

        public bool HasDroppedInitialFrames => writeCount > capacity;
        public IEnumerable<byte[]> InitialFrames => initialFrames;

        public VideoStreamChannel(int capacity)
        {
            this.capacity = capacity;
            initialFrames = [];
            channel = BoundedChannel.CreateSingleWriteMultiRead(capacity);
        }

        public void Open()
        {
            channel.Open();
        }

        public void Close()
        {
            channel.Close();
            writeCount = 0;
            initialFrames.Clear();
        }

        public void Write(byte[] data)
        {
            channel.Write(data);

            if (writeCount < capacity + numInitialFrames)
            {
                if (writeCount < numInitialFrames)
                {
                    initialFrames.Add(data);
                }
                writeCount++;
            }
        }

        public IAsyncEnumerable<byte[]> ReadAllAsync()
        {
            return channel.ReadAllAsync();
        }
    }
}
