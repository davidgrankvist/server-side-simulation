using ServerSideSimulation.Lib.Numbers;

namespace ServerSideSimulation.Lib.Test.Helpers
{
    internal static class EncoderTestHelpers
    {
        public static byte[] ToRuns(params ushort[] arr)
        {
            static byte[] ToSegments(ushort x, int i) => i % 2 == 0 ? [x.UpperByte(), x.LowerByte()] : [(byte)x];
            var result = arr.Select(ToSegments).SelectMany(x => x).ToArray();
            return result;
        }
    }
}
