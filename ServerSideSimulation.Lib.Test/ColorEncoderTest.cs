using ServerSideSimulation.Lib.Encoding;
using ServerSideSimulation.Lib.Test.Helpers;

namespace ServerSideSimulation.Lib.Test
{
    [TestClass]
    public class ColorEncoderTest
    {
        [TestMethod]
        public void ShouldConvertToIndexedColors()
        {
            var numPixels = 2;
            var encoder = new ColorEncoder(numPixels);
            var rawFrame = new byte[] { 0, 0, 0, 0, 1, 1, 1, 1 };
            var indexedColorData = new byte[] { 0, 1 };

            var encodedFrame = encoder.Encode(rawFrame);

            AssertExtensions.AreEqual(indexedColorData, encodedFrame);
        }
    }
}
