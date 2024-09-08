namespace ServerSideSimulation.Sim
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var settings = new RenderSettings(new BitmapChannel(10), 800, 800, 60);
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
