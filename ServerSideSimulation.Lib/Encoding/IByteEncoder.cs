namespace ServerSideSimulation.Lib.Encoding
{
    public interface IByteEncoder
    {
        byte[] Encode(byte[] data);
    }
}
