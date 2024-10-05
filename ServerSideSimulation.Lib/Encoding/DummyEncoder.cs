namespace ServerSideSimulation.Lib.Encoding
{
    public class DummyEncoder : IByteEncoder
    {
        public byte[] Encode(byte[] data)
        {
            return data;
        }
    }
}
