namespace ServerSideSimulation.Lib.Encoding
{
    /// <summary>
    /// Indicates how a frame is encoded.
    /// </summary>
    public enum FrameType : byte
    {
        /// <summary>
        /// Larger frame, marking the beginning of a frame sequence.
        /// </summary>
        IFrame = 0,
        /// <summary>
        /// Smaller frame within a sequence, computed from previous frames.
        /// </summary>
        PFrame = 1,
    }
}
