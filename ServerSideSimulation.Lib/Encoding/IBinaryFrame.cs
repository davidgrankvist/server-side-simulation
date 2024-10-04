namespace ServerSideSimulation.Lib.Encoding
{
    /// <summary>
    /// An encoded simulation frame.
    /// </summary>
    public interface IBinaryFrame
    {
        public FrameVersion Version { get; }

        public FrameType Type { get; }

        public byte[] Data { get; }
    }
}
