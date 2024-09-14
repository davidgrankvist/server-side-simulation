using ServerSideSimulation.Lib.Channels;
using ServerSideSimulation.Lib.Tcp;
using System.Net;

namespace ServerSideSimulation.Sim
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var settings = new RenderSettings(BoundedChannel.CreateSingleReadWrite(10), 800, 800, 30);
            var simulation = new Simulation(settings);
            var server = new TcpServer(IPEndPoint.Parse("127.0.0.1:12345"), new FrameSender(settings.Channel));

            settings.Channel.Open();

            var cancelSrc = new CancellationTokenSource();

            var serverTask = Task.Run(() => server.Run(cancelSrc.Token));
            simulation.Run();

            cancelSrc.Cancel();

            settings.Channel.Close();

            serverTask.Wait();
        }
    }
}
