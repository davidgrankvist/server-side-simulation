namespace ServerSideSimulation.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var channel = new VideoStreamChannel(100);
            var reader = new VideoStreamReader(channel);
            var server = new WebServer(channel);

            var cancelSrc = new CancellationTokenSource();

            channel.Open();

            var videoStreamTask = reader.Run(cancelSrc.Token);
            var webServerTask = server.Run();

            Task.WaitAny(webServerTask, videoStreamTask);

            cancelSrc.Cancel();

            Task.WaitAll(webServerTask, videoStreamTask);

            channel.Close();
        }
    }
}
