namespace ServerSideSimulation.Sim
{
    /// <summary>
    /// Converts RGBA pixels to one-byte color indicies.
    /// </summary>
    internal class ColorEncoder
    {
        private readonly byte[] buffer;

        public ColorEncoder(int bitmapSize)
        {
            buffer = new byte[bitmapSize];
        }

        public byte[] Encode(byte[] pixelData)
        {
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
