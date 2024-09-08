using System.Threading.Channels;

namespace ServerSideSimulation.Sim
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var channel = new BitmapChannel(10);
            var simulation = new Simulation(channel, 800, 800);
            simulation.Run();
        }
    }
}
