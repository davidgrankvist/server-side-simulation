using ServerSideSimulation.Lib.Channels;

namespace ServerSideSimulation.Sim
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var settings = new RenderSettings(BoundedChannel.CreateSingleReadWrite(10), 800, 800, 30);
            var simulation = new Simulation(settings);
            var encoder = new VideoEncoder(settings);

            settings.Channel.Open();

            var encoderTask = Task.Run(encoder.Run);
            simulation.Run();

            settings.Channel.Close();

            encoderTask.Wait();
        }
    }
}
