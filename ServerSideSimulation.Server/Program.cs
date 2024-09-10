namespace ServerSideSimulation.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new WebServer();
            server.Run().Wait();
        }
    }
}
