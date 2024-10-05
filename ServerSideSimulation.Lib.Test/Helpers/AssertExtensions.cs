namespace ServerSideSimulation.Lib.Test.Helpers
{
    internal static class AssertExtensions
    {
        public static void AreEqual<T>(IEnumerable<T> first, IEnumerable<T> second)
        {
            Assert.IsTrue(Enumerable.SequenceEqual(first, second));
        }
    }
}
