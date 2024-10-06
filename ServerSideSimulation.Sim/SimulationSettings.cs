namespace ServerSideSimulation.Sim
{
    internal class SimulationSettings
    {
        public readonly bool HeadlessMode;

        public readonly bool TransmitFrames;

        public SimulationSettings(bool headlessMode, bool transmitFrames)
        {
            HeadlessMode = headlessMode;
            TransmitFrames = transmitFrames;
        }

        public static readonly SimulationSettings HeadlessServer = new(true, true);

        public static readonly SimulationSettings WindowedServer = new(false, true);

        public static readonly SimulationSettings WindowedLocal = new(false, false);
    }
}
