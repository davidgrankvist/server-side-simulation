using System.Runtime.InteropServices;

namespace ServerSideSimulation.Sim.Platform.Raylib
{
    using Texture2D = Raylib.Texture;
    using RenderTexture2D = Raylib.RenderTexture;

    internal static class Raylib
    {
        // -------------- TYPES --------------

        [StructLayout(LayoutKind.Sequential)]
        public struct Color
        {
            public byte r;
            public byte g;
            public byte b;
            public byte a;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Texture
        {
            public uint id;
            public int width;
            public int height;
            public int mipmaps;
            public int format;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct Vector2
        {
            public float x;
            public float y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rectangle
        {
            public float x;
            public float y;
            public float width;
            public float height;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Image
        {
            public void* data;
            public int width;
            public int height;
            public int mipmaps;
            public int format;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct RenderTexture
        {
            public uint id;
            public Texture texture;
            public Texture depth;
        }

        // -------------- FUNCTIONS --------------

        [DllImport("raylib.dll")]
        public static extern void InitWindow(int width, int height, string title);

        [DllImport("raylib.dll")]
        public static extern void CloseWindow();

        [DllImport("raylib.dll")]
        public static extern bool WindowShouldClose();

        [DllImport("raylib.dll")]
        public static extern void SetTargetFPS(int fps);

        [DllImport("raylib.dll")]
        public static extern void ClearBackground(Color color);

        [DllImport("raylib.dll")]
        public static extern void BeginDrawing();

        [DllImport("raylib.dll")]
        public static extern void EndDrawing();

        [DllImport("raylib.dll")]
        public static extern void BeginTextureMode(RenderTexture2D target);

        [DllImport("raylib.dll")]
        public static extern void EndTextureMode();

        [DllImport("raylib.dll")]
        public static extern RenderTexture2D LoadRenderTexture(int width, int height);

        [DllImport("raylib.dll")]
        public static extern void DrawRectangle(int posX, int posY, int width, int height, Color color);

        [DllImport("raylib.dll")]
        public static extern void DrawRectangleRec(Rectangle rec, Color color);

        [DllImport("raylib.dll")]
        public static extern void DrawRectanglePro(Rectangle rec, Vector2 origin, float rotation, Color color);

        [DllImport("raylib.dll")]
        public static extern void DrawTexture(Texture2D texture, int posX, int posY, Color tint);

        [DllImport("raylib.dll")]
        public static extern void DrawTextureRec(Texture2D texture, Rectangle source, Vector2 position, Color tint);

        [DllImport("raylib.dll")]
        public static extern Image LoadImageFromTexture(Texture2D texture);

        [DllImport("raylib.dll")]
        public static extern void UnloadImage(Image image);

        // -------------- CONSTANTS --------------

        public static class Colors
        {
            public static readonly Color White = CreateColor(byte.MaxValue, byte.MaxValue, byte.MaxValue);
            public static readonly Color Black = CreateColor(0, 0, 0);
            public static readonly Color Red = CreateColor(byte.MaxValue, 0, 0);
            public static readonly Color Green = CreateColor(0, byte.MaxValue, 0);
            public static readonly Color Blue = CreateColor(0, 0, byte.MaxValue);

            public static Color CreateColor(byte r, byte g, byte b, byte a)
            {
                return new Color
                {
                    r = r,
                    g = g,
                    b = b,
                    a = a,
                };
            }

            public static Color CreateColor(byte r, byte g, byte b)
            {
                return CreateColor(r, g, b, byte.MaxValue);
            }
        }

    }
}
