namespace ServerSideSimulation.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var channel = new VideoStreamChannel(5);
            var reader = new VideoStreamReader(channel);
            var server = new WebServer(channel);
            var proxy = new VideoStreamProxy(channel);

            var cancelSrc = new CancellationTokenSource();

            channel.Open();

            var videoStreamTask = reader.Run(cancelSrc.Token);
            var webServerTask = server.Run();
            var proxyTask = proxy.Run(cancelSrc.Token);

            var tasks = new Task[] { videoStreamTask, webServerTask, proxyTask };

            Task.WaitAny(tasks);

            cancelSrc.Cancel();

            Task.WaitAll(tasks);

            channel.Close();
        }
    }
}
