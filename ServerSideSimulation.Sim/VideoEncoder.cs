namespace ServerSideSimulation.Sim
{
    internal class VideoEncoder
    {
        private BitmapChannel channel;
        public VideoEncoder(BitmapChannel channel)
        {
            this.channel = channel;
        }

        public async void Run()
        {
            await foreach (var frame in channel.ReadAllAsync())
            {
                // do the encoding here
            }
        }
    }
}
