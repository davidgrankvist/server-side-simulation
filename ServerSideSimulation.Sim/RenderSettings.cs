using ServerSideSimulation.Lib.Channels;

namespace ServerSideSimulation.Sim
{
    internal class RenderSettings
    {
        public readonly BoundedChannel Channel;
        public readonly int ScreenWidth;
        public readonly int ScreenHeight;
        public readonly int Fps;

        public RenderSettings(BoundedChannel channel, int screenWidth, int screenHeight, int fps)
        {
            Channel = channel;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            Fps = fps;
        }
    }
}
