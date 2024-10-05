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
            var encoder = new FrameEncoder(numPixels, 1, new DummyEncoder());
            var data = new byte[] { 0, 0, 0, 0, 1, 1, 1, 1 };
            var expected = new byte[] { 0, 1 };

            var iFrame = encoder.Encode(data);

            AssertExtensions.AreEqual(expected, iFrame.Data);
        }

        [TestMethod]
        public void ShouldEncodePFramesThatCanBeReconstructed()
        {
            var numPixels = 2;
            var iFrameFrequency = 3;
            var encoder = new FrameEncoder(numPixels, iFrameFrequency, new DummyEncoder());
            var deltaEncoder = new DeltaEncoder(numPixels);

            var rawFrame1 = new byte[] { 0, 0, 0, 0, 1, 1, 1, 1 };
            var rawFrame2 = new byte[] { 2, 2, 2, 2, 3, 3, 3, 3 };
            var rawFrame3 = new byte[] { 4, 4, 4, 4, 5, 5, 5, 5 };
            var indexedColorData1 = new byte[] { 0, 1 };
            var indexedColorData2 = new byte[] { 2, 3 };
            var indexedColorData3 = new byte[] { 4, 5 };

            var iFrame = encoder.Encode(rawFrame1);
            var pFrame1 = encoder.Encode(rawFrame2);
            var pFrame2 = encoder.Encode(rawFrame3);
            // simulate client side decoding where the received data is an I-Frame followed by deltas
            var deltaDecoded1 = deltaEncoder.Encode(iFrame.Data, pFrame1.Data);
            var deltaDecoded2 = deltaEncoder.Encode(deltaDecoded1, pFrame2.Data);

            AssertExtensions.AreEqual(deltaDecoded1, indexedColorData2);
            AssertExtensions.AreEqual(deltaDecoded2, indexedColorData3);
        }

        [TestMethod]
        public void ShouldOutputIFrameEveryNthFrame()
        {
            var numPixels = 2;
            var iFrameFrequency = 3;
            var encoder = new FrameEncoder(numPixels, iFrameFrequency, new DummyEncoder());
            var rawFrame = new byte[] { 0, 0, 0, 0, 1, 1, 1, 1 };

            var frame1 = encoder.Encode(rawFrame);
            var frame2 = encoder.Encode(rawFrame);
            var frame3 = encoder.Encode(rawFrame);
            var frame4 = encoder.Encode(rawFrame);

            Assert.AreEqual(FrameType.IFrame, frame1.Type);
            Assert.AreEqual(FrameType.PFrame, frame2.Type);
            Assert.AreEqual(FrameType.PFrame, frame3.Type);
            Assert.AreEqual(FrameType.IFrame, frame4.Type);
        }

        [TestMethod]
        public void ShouldMaintainUnmodifiedPixels()
        {
            var numPixels = 2;
            var iFrameFrequency = 3;
            var encoder = new FrameEncoder(numPixels, iFrameFrequency, new DummyEncoder());
            var deltaEncoder = new DeltaEncoder(numPixels);

            var rawFrame1 = new byte[] { 255, 255, 255, 255, 1, 1, 1, 1 };
            var rawFrame2 = new byte[] { 255, 255, 255, 255, 2, 2, 2, 2 };
            var rawFrame3 = new byte[] { 255, 255, 255, 255, 3, 3, 3, 3 };
            var indexedColorData1 = new byte[] { 255, 1 };
            var indexedColorData2 = new byte[] { 255, 2 };
            var indexedColorData3 = new byte[] { 255, 3 };

            var iFrame = encoder.Encode(rawFrame1);
            var pFrame1 = encoder.Encode(rawFrame2);
            var pFrame2 = encoder.Encode(rawFrame3);
            // simulate client side decoding where the received data is an I-Frame followed by deltas
            var deltaDecoded1 = deltaEncoder.Encode(iFrame.Data, pFrame1.Data);
            var deltaDecoded2 = deltaEncoder.Encode(deltaDecoded1, pFrame2.Data);

            AssertExtensions.AreEqual(deltaDecoded1, indexedColorData2);
            AssertExtensions.AreEqual(deltaDecoded2, indexedColorData3);
        }

        [TestMethod]
        public void ShouldEncodeIFrameWithRle()
        {
            var numPixels = 2;
            var encoder = new FrameEncoder(numPixels, 1, new RunLengthEncoder());
            var data = new byte[] { 0, 0, 0, 0, 1, 1, 1, 1 };
            var expected = EncoderTestHelpers.ToRuns(1, 0, 1, 1);

            var iFrame = encoder.Encode(data);

            AssertExtensions.AreEqual(expected, iFrame.Data);
        }
    }
}