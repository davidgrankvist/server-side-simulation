﻿using ServerSideSimulation.Sim.Platform.Raylib;
using System.Runtime.InteropServices;

namespace ServerSideSimulation.Sim
{
    internal class Simulation
    {
        private BitmapChannel channel;
        private bool headlessMode;
        private int screenWidth;
        private int screenHeight;

        public Simulation(BitmapChannel channel, int screenWidth, int screenHeight, bool headlessMode = false)
        {
            this.channel = channel;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.headlessMode = headlessMode;
        }

        public void Run()
        {
            var title = "Simulation";

            var textureRec = new Raylib.Rectangle
            {
                x = 0,
                y = 0,
                width = screenWidth,
                height = screenHeight
            };
            var texturePos = new Raylib.Vector2();

            var rect = new Raylib.Rectangle
            {
                x = 400,
                y = 400,
                width = 120,
                height = 200
            };
            var angle = 0f;
            var origin = new Raylib.Vector2
            {
                x = rect.width / 2,
                y = rect.height / 2,
            };
            var angleDelta = 0.5f;

            if (headlessMode)
            {
                // raylib is not built for headless mode, so allocate a small buffer instead
                Raylib.InitWindow(1, 1, title);
            }
            else
            {
                Raylib.InitWindow(screenWidth, screenHeight, title);
            }

            var renderTexture = Raylib.LoadRenderTexture(screenWidth, screenHeight);
            Raylib.SetTargetFPS(60);
            while (!Raylib.WindowShouldClose())
            {
                // primary rendering to target texture
                Raylib.BeginTextureMode(renderTexture);
                Raylib.ClearBackground(Raylib.Colors.White);
                angle += angleDelta;
                Raylib.DrawRectanglePro(rect, origin, angle, Raylib.Colors.Blue);
                Raylib.EndTextureMode();

                // bitmap extraction
                var image = Raylib.LoadImageFromTexture(renderTexture.texture);
                TransmitImage(image);
                Raylib.UnloadImage(image);

                // always use begin/end drawing to allow closing the window
                Raylib.BeginDrawing();
                if (!headlessMode)
                {
                    // debug render to window
                    Raylib.ClearBackground(Raylib.Colors.White);
                    Raylib.DrawTextureRec(renderTexture.texture, textureRec, texturePos, Raylib.Colors.White);
                }
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        private byte[] IntPtrToByteArray(IntPtr ptr, int length)
        {
            var bytes = new byte[length];
            Marshal.Copy(ptr, bytes, 0, bytes.Length);
            return bytes;
        }

        private void TransmitImage(Raylib.Image image)
        {
            const int bytesPerPixel = 4; // RGBA
            var bitmap = IntPtrToByteArray(image.data, image.width * image.height * bytesPerPixel);
            channel.Write(bitmap);
        }
    }
}
