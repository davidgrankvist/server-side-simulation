namespace ServerSideSimulation.Sim
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var channel = new BitmapChannel(10);
            channel.Open();

            var simulation = new Simulation(channel, 800, 800);
            var encoder = new VideoEncoder(channel);

            var encoderTask = Task.Run(encoder.Run);
            simulation.Run();
            channel.Close();

            encoderTask.Wait();
        }
    }
}
