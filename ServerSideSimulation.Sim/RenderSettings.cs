namespace ServerSideSimulation.Sim
{
    internal class RenderSettings
    {
        public readonly BitmapChannel Channel;
        public readonly int ScreenWidth;
        public readonly int ScreenHeight;
        public readonly int Fps;

        public RenderSettings(BitmapChannel channel, int screenWidth, int screenHeight, int fps)
        {
            Channel = channel;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            Fps = fps;
        }
    }
}
