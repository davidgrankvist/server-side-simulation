namespace ServerSideSimulation.Lib.Encoding
{
    public class DeltaEncoder
    {
        private readonly int numPixels;

        public DeltaEncoder(int numPixels)
        {
            this.numPixels = numPixels;
        }

        public byte[] Encode(byte[] first, byte[] second)
        {
            var buffer = new byte[numPixels];
            for (var i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)(first[i] ^ second[i]);
            }

            return buffer;
        }
    }
}
