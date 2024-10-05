using ServerSideSimulation.Lib.Numbers;

namespace ServerSideSimulation.Lib.Encoding
{
    public class RunLengthEncoder : IByteEncoder
    {
        private readonly int runLength;

        public RunLengthEncoder(int runLength)
        {
            this.runLength = runLength;
        }

        public RunLengthEncoder() : this(ushort.MaxValue)
        {
        }

        public byte[] Encode(byte[] data)
        {
            var outputBuffer = new byte[3 * data.Length];

            ushort count = 0;
            var current = data[0];
            var iOut = 0;

            for (var i = 0; i < data.Length; i++)
            {
                var b = data[i];

                if (b == current && count < runLength && i < data.Length)
                {
                    count++;
                }
                else
                {
                    outputBuffer[iOut++] = count.UpperByte();
                    outputBuffer[iOut++] = count.LowerByte();
                    outputBuffer[iOut++] = current;

                    count = 1;
                    current = b;
                }

                if (i == data.Length - 1)
                {
                    outputBuffer[iOut++] = count.UpperByte();
                    outputBuffer[iOut++] = count.LowerByte();
                    outputBuffer[iOut++] = current;
                }
            }

            var outputLength = iOut;
            var result = new byte[outputLength];
            Array.Copy(outputBuffer, result, outputLength);

            return result;
        }
    }
}
