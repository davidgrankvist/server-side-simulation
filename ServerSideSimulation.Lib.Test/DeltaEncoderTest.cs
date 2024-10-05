using ServerSideSimulation.Lib.Encoding;
using ServerSideSimulation.Lib.Test.Helpers;

namespace ServerSideSimulation.Lib.Test
{
    [TestClass]
    public class DeltaEncoderTest
    {
        [TestMethod]
        public void ShouldDoXor()
        {
            var numPixels = 4;
            var encoder = new DeltaEncoder(numPixels);
            var frame1 = new byte[] { 0, 0, 1, 1 };
            var frame2 = new byte[] { 0, 1, 0, 1 };
            var expected = new byte[] { 0, 1, 1, 0 };

            var encoded = encoder.Encode(frame1, frame2);

            AssertExtensions.AreEqual(expected, encoded);
        }

        [TestMethod]
        public void ShouldBeReversible()
        {
            var numPixels = 4;
            var encoder = new DeltaEncoder(numPixels);
            var frame1 = new byte[] { 0, 0, 1, 1 };
            var frame2 = new byte[] { 0, 1, 0, 1 };

            var encoded1 = encoder.Encode(frame1, frame2);
            var encoded2 = encoder.Encode(frame1, encoded1);

            AssertExtensions.AreEqual(frame2, encoded2);
        }
    }
}
