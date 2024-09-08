namespace ServerSideSimulation.Sim
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var screenWidth = 800;
            var screenHeight = 800;
            var fps = 60;
            var channel = new BitmapChannel(10);
            var simulation = new Simulation(channel, screenWidth, screenHeight, fps);
            var encoder = new VideoEncoder(channel, screenWidth, screenHeight, fps);

            channel.Open();

            var encoderTask = Task.Run(encoder.Run);
            simulation.Run();

            channel.Close();

            encoderTask.Wait();
        }
    }
}
