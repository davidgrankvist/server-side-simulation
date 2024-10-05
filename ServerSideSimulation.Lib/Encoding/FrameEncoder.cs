using ServerSideSimulation.Lib.Encoding;

namespace ServerSideSimulation.Sim
{
    public class FrameEncoder
    {
        private readonly ColorEncoder colorEncoder;
        private readonly DeltaEncoder deltaEncoder;

        private int frameIndex = 0;
        private readonly int iFrameFrequency;
        private byte[] prevIndexedColorsPixels;

        public FrameEncoder(int numPixels, int iFrameFrequency)
        {
            colorEncoder = new ColorEncoder(numPixels);
            deltaEncoder = new DeltaEncoder(numPixels);
            this.iFrameFrequency = iFrameFrequency;
        }

        public Frame Encode(byte[] pixelData)
        {
            var indexedColors = colorEncoder.Encode(pixelData);

            Frame frame;
            if (frameIndex == 0)
            {
                prevIndexedColorsPixels = indexedColors;
                frame = Frame.CreateIFrame(indexedColors);
            }
            else
            {
                var colorDeltas = deltaEncoder.Encode(prevIndexedColorsPixels, indexedColors);
                prevIndexedColorsPixels = indexedColors;
                // TODO(optimize): use RLE
                frame = Frame.CreatePFrame(colorDeltas);
            }

            frameIndex = (frameIndex + 1) % iFrameFrequency;
            return frame;
        }
    }
}
