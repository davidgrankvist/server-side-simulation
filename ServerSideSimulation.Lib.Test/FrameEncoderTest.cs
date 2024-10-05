using ServerSideSimulation.Lib.Encoding;
using ServerSideSimulation.Lib.Test.Helpers;
using ServerSideSimulation.Sim;

namespace ServerSideSimulation.Lib.Test
{
    [TestClass]
    public class FrameEncoderTest
    {
        [TestMethod]
        public void ShouldEncodeValidIFrame()
        {
            var numPixels = 2;
            var encoder = new FrameEncoder(numPixels, 1);
            var data = new byte[] { 0, 0, 0, 0, 1, 1, 1, 1 };
            var expected = new byte[] { 0, 1 };

            var iFrame = encoder.Encode(data);

            AssertExtensions.AreEqual(expected, iFrame.Data);
        }

        [TestMethod]
        public void ShouldEncodeValidPFrame()
        {
            var numPixels = 2;
            var encoder = new FrameEncoder(numPixels, 2);
            var deltaEncoder = new DeltaEncoder(numPixels);

            var rawFrame1 = new byte[] { 0, 0, 0, 0, 1, 1, 1, 1 };
            var rawFrame2 = new byte[] { 2, 2, 2, 2, 3, 3, 3, 3 };

            var indexedColorData1 = new byte[] { 0, 1 };
            var indexedColorData2 = new byte[] { 2, 3 };

            var iFrame = encoder.Encode(rawFrame1);
            var pFrame = encoder.Encode(rawFrame2);

            var deltaDecoded = deltaEncoder.Encode(indexedColorData1, pFrame.Data);

            AssertExtensions.AreEqual(deltaDecoded, indexedColorData2);
        }
    }
}