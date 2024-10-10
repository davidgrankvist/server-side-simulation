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

        [StructLayout(LayoutKind.Sequential)]
        public struct Shader
        {
            public uint id;
            public IntPtr locs;
        }

        public enum ShaderUniformDataType
        {
            SHADER_UNIFORM_FLOAT = 0,       // Shader uniform type: float
            SHADER_UNIFORM_VEC2,            // Shader uniform type: vec2 (2 float)
            SHADER_UNIFORM_VEC3,            // Shader uniform type: vec3 (3 float)
            SHADER_UNIFORM_VEC4,            // Shader uniform type: vec4 (4 float)
            SHADER_UNIFORM_INT,             // Shader uniform type: int
            SHADER_UNIFORM_IVEC2,           // Shader uniform type: ivec2 (2 int)
            SHADER_UNIFORM_IVEC3,           // Shader uniform type: ivec3 (3 int)
            SHADER_UNIFORM_IVEC4,           // Shader uniform type: ivec4 (4 int)
            SHADER_UNIFORM_SAMPLER2D        // Shader uniform type: sampler2d
        }

        // Mesh, vertex data and vao/vbo
        [StructLayout(LayoutKind.Sequential)]
        public struct Mesh
        {
            public int vertexCount;        // Number of vertices stored in arrays
            public int triangleCount;      // Number of triangles stored (indexed or not)

            // Vertex attributes data
            public IntPtr vertices;        // Vertex position (XYZ - 3 components per vertex) (shader-location = 0)
            public IntPtr texcoords;       // Vertex texture coordinates (UV - 2 components per vertex) (shader-location = 1)
            public IntPtr texcoords2;      // Vertex texture second coordinates (UV - 2 components per vertex) (shader-location = 5)
            public IntPtr normals;         // Vertex normals (XYZ - 3 components per vertex) (shader-location = 2)
            public IntPtr tangents;        // Vertex tangents (XYZW - 4 components per vertex) (shader-location = 4)
            public IntPtr colors;      // Vertex colors (RGBA - 4 components per vertex) (shader-location = 3)
            public IntPtr indices;    // Vertex indices (in case vertex data comes indexed)

            // Animation vertex data
            public IntPtr animVertices;    // Animated vertex positions (after bones transformations)
            public IntPtr animNormals;     // Animated normals (after bones transformations)
            public IntPtr boneIds; // Vertex bone ids, max 255 bone ids, up to 4 bones influence by vertex (skinning)
            public IntPtr boneWeights;     // Vertex bone weight, up to 4 bones influence by vertex (skinning)

            // OpenGL identifiers
            public uint vaoId;     // OpenGL Vertex Array Object id
            public IntPtr vboId;    // OpenGL Vertex Buffer Objects id (default vertex data)
        }

        // Material, includes shader and maps
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Material
        {
            public Shader shader;          // Material shader
            public MaterialMap* maps;      // Material maps array (MAX_MATERIAL_MAPS)
            public fixed float @params[4];                       // Material generic parameters
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Matrix
        {
            public float m0, m4, m8, m12;  // Matrix first row (4 components)
            public float m1, m5, m9, m13;  // Matrix second row (4 components)
            public float m2, m6, m10, m14; // Matrix third row (4 components)
            public float m3, m7, m11, m15; // Matrix fourth row (4 components)
        }

        // MaterialMap
        [StructLayout(LayoutKind.Sequential)]
        public struct MaterialMap
        {
            public Texture2D texture;      // Material map texture
            public Color color;            // Material map color
            public float value;            // Material map value
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

        [DllImport("raylib.dll")]
        public static extern Shader LoadShader(string vsFileName, string fsFileName);

        [DllImport("raylib.dll")]
        public static extern void UnloadShader(Shader shader);

        [DllImport("raylib.dll")]
        public static extern void BeginShaderMode(Shader shader);

        [DllImport("raylib.dll")]
        public static extern void EndShaderMode();

        [DllImport("raylib.dll")]
        public static extern unsafe void SetShaderValue(Shader shader, int locIndex, void* value, ShaderUniformDataType uniformType);

        [DllImport("raylib.dll")]
        public static extern unsafe void UploadMesh(Mesh* mesh, bool dynamic);

        [DllImport("raylib.dll")]
        public static extern unsafe void DrawMesh(Mesh mesh, Material material, Matrix transform);

        [DllImport("raylib.dll")]
        public static extern float GetFrameTime();

        [DllImport("raylib.dll")]
        public static extern int GetShaderLocation(Shader shader, string uniformName);

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

        public static class Matrices
        {
            public static readonly Matrix Identity = new Matrix
            {
                m0 = 1,
                m5 = 1,
                m10 = 1,
                m15 = 1
            };
        }

        public static void SetMeshVertices(ref Mesh mesh, float[] vertices)
        {
            mesh.vertexCount = vertices.Length / 3;
            mesh.vertices = Marshal.UnsafeAddrOfPinnedArrayElement(vertices, 0);
        }

        public static void SetMeshIndices(ref Mesh mesh, ushort[] indices)
        {
            mesh.triangleCount = indices.Length / 3;
            mesh.indices = Marshal.UnsafeAddrOfPinnedArrayElement(indices, 0);
        }

        public static unsafe void UploadMeshHelper(ref Mesh mesh, bool dynamic)
        {
            fixed (Mesh* pmesh = &mesh)
            {
                UploadMesh(pmesh, dynamic);
            }
        }

        public static unsafe void SetMaterialParams(ref Material material, float[] fs)
        {
            material.@params[0] = fs[0];
            material.@params[1] = fs[1];
            material.@params[2] = fs[2];
            material.@params[3] = fs[3];
        }

        public static unsafe void SetMaterialMaps(ref Material material, MaterialMap[] maps)
        {
            fixed (MaterialMap* pmap = maps)
            {
                material.maps = pmap;
            }
        }

        public static unsafe void SetShaderValueHelper<T>(Shader shader, int locIndex, T value)
        {
            if (value is float f)
            {
                SetShaderValue(shader, locIndex, &f, ShaderUniformDataType.SHADER_UNIFORM_FLOAT);
            }
            else
            {
                throw new InvalidOperationException("Unsupported shader value type");
            }
        }

    }
}
