using ServerSideSimulation.Lib.Encoding;
using ServerSideSimulation.Lib.Test.Helpers;

namespace ServerSideSimulation.Lib.Test
{
    [TestClass]
    public class RunLengthEncoderTest
    {
        [TestMethod]
        public void ShouldEncodeRepetitions()
        {
            var input = new byte[] { 0, 0, 0, 1, 1, 1, 1 };
            var expected = EncoderTestHelpers.ToRuns(3, 0, 4, 1);
            var encoder = new RunLengthEncoder();

            var result = encoder.Encode(input);

            AssertExtensions.AreEqual(expected, result);
        }

        [TestMethod]
        public void ShouldEncodeUnique()
        {
            var input = new byte[] { 3, 4, 5, 6 };
            var expected = EncoderTestHelpers.ToRuns(1, 3, 1, 4, 1, 5, 1, 6);
            var encoder = new RunLengthEncoder();

            var result = encoder.Encode(input);

            AssertExtensions.AreEqual(expected, result);
        }

        [TestMethod]
        public void ShouldEncodeMixed()
        {
            var input = new byte[] { 0, 0, 0, 1, 1, 2, 3, 3, 3, 3 };
            var expected = EncoderTestHelpers.ToRuns(3, 0, 2, 1, 1, 2, 4, 3);
            var encoder = new RunLengthEncoder();

            var result = encoder.Encode(input);

            AssertExtensions.AreEqual(expected, result);
        }

        [TestMethod]
        public void ShouldRespectRunLength()
        {
            var runLength = 2;
            var input = new byte[] { 3, 3, 3, 4, 4, 4 };
            var expected = EncoderTestHelpers.ToRuns(2, 3, 1, 3, 2, 4, 1, 4);
            var encoder = new RunLengthEncoder(runLength);

            var result = encoder.Encode(input);

            AssertExtensions.AreEqual(expected, result);
        }
    }
}
