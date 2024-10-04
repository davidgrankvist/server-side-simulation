using ServerSideSimulation.Lib.Encoding;

namespace ServerSideSimulation.Sim
{
    /// <summary>
    /// A frame structure to encode.
    /// </summary>
    internal readonly struct Frame : IBinaryFrame
    {
        public FrameVersion Version {get; }

        public FrameType Type {get; }

        public byte[] Data { get; }

        private Frame(FrameVersion version, FrameType type, byte[] data)
        {
            Version = version;
            Type = type;
            Data = data;
        }

        public byte[] ToBytes()
        {
            const int NumHeaderBytes = 6;
            var bytes = new byte[NumHeaderBytes + Data.Length];
            bytes[0] = (byte)Version;
            bytes[1] = (byte)Type;
            bytes[2] = (byte)((Data.Length >> 24) & 0xFF);
            bytes[3] = (byte)((Data.Length >> 16) & 0xFF);
            bytes[4] = (byte)((Data.Length >> 8) & 0xFF);
            bytes[5] = (byte)((Data.Length) & 0xFF);
            Array.Copy(Data, 0, bytes, NumHeaderBytes, Data.Length);

            return bytes;
        }

        public static Frame CreateIFrame(byte[] data)
        {
            return new Frame(FrameVersion.V0, FrameType.IFrame, data);
        }

        public static Frame CreatePFrame(byte[] data)
        {
            return new Frame(FrameVersion.V0, FrameType.PFrame, data);
        }
    }
}
