namespace ServerSideSimulation.Lib.Numbers
{
    public static class UShortExtensions
    {
        public static byte UpperByte(this ushort n)
        {
            return (byte)(n >> 8);
        }

        public static byte LowerByte(this ushort n)
        {
            return (byte)(n & 0x00FF);
        }
    }
}
