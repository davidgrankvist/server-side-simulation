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
            public IntPtr data;
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

        public enum TraceLogLevel
        {
            LOG_ALL = 0,
            LOG_TRACE,
            LOG_DEBUG,
            LOG_INFO,
            LOG_WARNING,
            LOG_ERROR,
            LOG_FATAL,
            LOG_NONE,
        }

        [Flags]
        public enum ConfigFlags
        {
            FLAG_VSYNC_HINT = 0x00000040,   // Set to try enabling V-Sync on GPU
            FLAG_FULLSCREEN_MODE = 0x00000002,   // Set to run program in fullscreen
            FLAG_WINDOW_RESIZABLE = 0x00000004,   // Set to allow resizable window
            FLAG_WINDOW_UNDECORATED = 0x00000008,   // Set to disable window decoration (frame and buttons)
            FLAG_WINDOW_HIDDEN = 0x00000080,   // Set to hide window
            FLAG_WINDOW_MINIMIZED = 0x00000200,   // Set to minimize window (iconify)
            FLAG_WINDOW_MAXIMIZED = 0x00000400,   // Set to maximize window (expanded to monitor)
            FLAG_WINDOW_UNFOCUSED = 0x00000800,   // Set to window non focused
            FLAG_WINDOW_TOPMOST = 0x00001000,   // Set to window always on top
            FLAG_WINDOW_ALWAYS_RUN = 0x00000100,   // Set to allow windows running while minimized
            FLAG_WINDOW_TRANSPARENT = 0x00000010,   // Set to allow transparent framebuffer
            FLAG_WINDOW_HIGHDPI = 0x00002000,   // Set to support HighDPI
            FLAG_WINDOW_MOUSE_PASSTHROUGH = 0x00004000, // Set to support mouse passthrough, only supported when FLAG_WINDOW_UNDECORATED
            FLAG_BORDERLESS_WINDOWED_MODE = 0x00008000, // Set to run program in borderless windowed mode
            FLAG_MSAA_4X_HINT = 0x00000020,   // Set to try enabling MSAA 4X
            FLAG_INTERLACED_HINT = 0x00010000    // Set to try enabling interlaced video format (for V3D)
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

        [DllImport("raylib.dll")]
        public static extern void SetTraceLogLevel(TraceLogLevel logLevel);

        [DllImport("raylib.dll")]
        public static extern void SetConfigFlags(ConfigFlags flags);

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
