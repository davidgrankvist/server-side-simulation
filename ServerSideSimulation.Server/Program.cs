namespace ServerSideSimulation.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new WebSocketServer();
            server.Run().Wait();
        }
    }
}
