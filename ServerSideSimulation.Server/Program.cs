using ServerSideSimulation.Lib.Channels;
using ServerSideSimulation.Lib.Tcp;
using System.Net;

namespace ServerSideSimulation.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var channel = BoundedChannel.CreateSingleWriteMultiRead(5);
            var reader = new TcpClientWrapper(IPEndPoint.Parse("127.0.0.1:12345"), new FrameReceiver(channel, 800 * 800 * 4));
            var server = new WebServer(channel);
            var proxy = new TcpServer(IPEndPoint.Parse("127.0.0.1:12346"), new FrameSender(channel));

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
