namespace ServerSideSimulation.Lib.Encoding
{
    /// <summary>
    /// A view into encoded frame data.
    /// </summary>
    public class FrameView : IBinaryFrame
    {
        private byte[]? data;

        public FrameVersion Version => GetVersion();

        public FrameType Type => GetFrameType();

        public byte[] Data => throw new NotImplementedException();

        public FrameView()
        {
        }

        public FrameView(byte[] data)
        {
            this.data = data;
        }

        public void SetData(byte[] data)
        {
            this.data = data;
        }

        private FrameVersion GetVersion()
        {
            return (FrameVersion)data![0];
        }

        private FrameType GetFrameType()
        {
            return (FrameType)data![1];
        }
    }
}
