using ServerSideSimulation.Lib.Encoding;

namespace ServerSideSimulation.Sim
{
    public class FrameEncoder
    {
        private readonly ColorEncoder colorEncoder;
        private readonly DeltaEncoder deltaEncoder;
        private readonly IByteEncoder runLengthEncoder;

        private int frameIndex = 0;
        private readonly int iFrameFrequency;
        private byte[] prevIndexedColorsPixels;

        public FrameEncoder(
            int numPixels,
            int iFrameFrequency,
            IByteEncoder runLengthEncoder)
        {
            colorEncoder = new ColorEncoder(numPixels);
            deltaEncoder = new DeltaEncoder(numPixels);
            this.runLengthEncoder = runLengthEncoder;
            this.iFrameFrequency = iFrameFrequency;
        }

        public Frame Encode(byte[] pixelData)
        {
            var indexedColors = colorEncoder.Encode(pixelData);

            Frame frame;
            if (frameIndex == 0)
            {
                prevIndexedColorsPixels = indexedColors;
                var rleData = runLengthEncoder.Encode(indexedColors);
                frame = Frame.CreateIFrame(rleData);
            }
            else
            {
                var colorDeltas = deltaEncoder.Encode(prevIndexedColorsPixels, indexedColors);
                prevIndexedColorsPixels = indexedColors;
                var rleData = runLengthEncoder.Encode(colorDeltas);
                frame = Frame.CreatePFrame(rleData);
            }

            frameIndex = (frameIndex + 1) % iFrameFrequency;
            return frame;
        }
    }
}
