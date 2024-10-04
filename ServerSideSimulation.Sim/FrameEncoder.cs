namespace ServerSideSimulation.Sim
{
    internal class FrameEncoder
    {
        private readonly ColorEncoder colorEncoder;
        private readonly byte[] buffer;

        private int frameIndex = 0;
        private readonly int iFrameFrequency;

        public FrameEncoder(int numPixels, int iFrameFrequency)
        {
            colorEncoder = new ColorEncoder(numPixels);
            buffer = new byte[numPixels];
            this.iFrameFrequency = iFrameFrequency;
        }

        public Frame Encode(byte[] pixelData)
        {
            var indexedColors = colorEncoder.Encode(pixelData);

            Frame frame;
            if (frameIndex == 0)
            {
                frame = Frame.CreateIFrame(indexedColors);
            }
            else
            {
                // TODO(optimize): do delta encoding here
                frame = Frame.CreatePFrame(indexedColors);
            }

            frameIndex = (frameIndex + 1) % iFrameFrequency;
            return frame;
        }
    }
}
