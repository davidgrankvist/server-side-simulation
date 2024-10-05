namespace ServerSideSimulation.Lib.Encoding
{
    /// <summary>
    /// Converts RGBA pixels to one-byte color indicies.
    /// </summary>
    public class ColorEncoder : IByteEncoder
    {
        private readonly int bitmapSize;

        public ColorEncoder(int bitmapSize)
        {
            this.bitmapSize = bitmapSize;
        }

        public byte[] Encode(byte[] pixelData)
        {
            var buffer = new byte[bitmapSize];
            const int bytesPerPixel = 4;
            for (int iBuffer = 0, iPixelData = 0; iBuffer < buffer.Length; iBuffer++, iPixelData += bytesPerPixel)
            {
                // assume gray scale values, meaning only the first color is relevant
                buffer[iBuffer] = pixelData[iPixelData];
            }

            return buffer;
        }
    }
}
