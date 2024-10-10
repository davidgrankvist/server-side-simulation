using ServerSideSimulation.Sim.Platform.Raylib;
using System.Runtime.InteropServices;

namespace ServerSideSimulation.Sim
{
    internal class Simulation
    {
        private readonly RenderSettings settings;
        private readonly SimulationSettings simSettings;

        public Simulation(RenderSettings settings, SimulationSettings simSettings)
        {
            this.settings = settings;
            this.simSettings = simSettings;
        }

        public void Run()
        {
            var title = "Simulation";

            var textureRec = new Raylib.Rectangle
            {
                x = 0,
                y = 0,
                width = settings.ScreenWidth,
                height = settings.ScreenHeight
            };
            var texturePos = new Raylib.Vector2();

            Raylib.SetTraceLogLevel(Raylib.TraceLogLevel.LOG_NONE);
            if (simSettings.HeadlessMode)
            {
                Raylib.SetConfigFlags(Raylib.ConfigFlags.FLAG_WINDOW_HIDDEN);
                Raylib.InitWindow(1, 1, title);
            }
            else
            {
                Raylib.InitWindow(settings.ScreenWidth, settings.ScreenHeight, title);
            }

            var renderTexture = Raylib.LoadRenderTexture(settings.ScreenWidth, settings.ScreenHeight);
            var shader = Raylib.LoadShader(Path.Combine("Shaders", "vertex.glsl"), Path.Combine("Shaders", "fragment.glsl"));

            // define a plane mesh using 4 vertices and two triangles
            float[] vertices = {
                -0.5f, -0.5f, 0.0f,
                 0.5f, -0.5f, 0.0f,
                -0.5f,  0.5f, 0.0f,
                 0.5f,  0.5f, 0.0f 
            };
            ushort[] triangleIndices = {
                0, 1, 2,
                1, 3, 2 
            };
            var mesh = new Raylib.Mesh();
            Raylib.SetMeshVertices(ref mesh, vertices);
            Raylib.SetMeshIndices(ref mesh, triangleIndices);
            Raylib.UploadMeshHelper(ref mesh, true);

            // create an unused material in order to call DrawMesh later
            var materialMap = new Raylib.MaterialMap();
            materialMap.texture = renderTexture.texture;
            var materialMaps = new Raylib.MaterialMap[] { materialMap };
            var material = new Raylib.Material();
            material.shader = shader;
            Raylib.SetMaterialParams(ref material, [0, 0, 0, 0]);
            Raylib.SetMaterialMaps(ref material, materialMaps);

            var angleLocation = Raylib.GetShaderLocation(shader, "angle");
            var colorLocation = Raylib.GetShaderLocation(shader, "color");

            var angle = 0f;
            var angleSign = 1f;
            var color = 0f;
            var colorSign = 1;
            var timeScale = 0.5f;

            Raylib.SetTargetFPS(settings.Fps);
            while (!Raylib.WindowShouldClose())
            {
                angle += Raylib.GetFrameTime() * angleSign * timeScale;
                if (angle >= MathF.PI / 4)
                {
                   angle = MathF.PI / 4;
                   angleSign = -angleSign;
                }
                else if (angle <= 0f)
                {
                    angle = 0;
                    angleSign = -angleSign;
                }

                color += Raylib.GetFrameTime() * colorSign * timeScale;
                if (color >= 0.5f)
                {
                    color = 0.5f;
                    colorSign = -colorSign;
                }
                else if (color <= 0f)
                {
                    color = 0f;
                    colorSign = -colorSign;
                }

                Raylib.SetShaderValueHelper(shader, angleLocation, angle);
                Raylib.SetShaderValueHelper(shader, colorLocation, color);

                // primary rendering to target texture
                Raylib.BeginTextureMode(renderTexture);
                Raylib.ClearBackground(Raylib.Colors.White);
                Raylib.BeginShaderMode(shader);

                // invoke a draw call to make sure rendering is done - the actual animation happens within the shaders
                Raylib.DrawMesh(mesh, material, Raylib.Matrices.Identity);

                Raylib.EndShaderMode();
                Raylib.EndTextureMode();

                // bitmap extraction
                if (simSettings.TransmitFrames)
                {
                    var image = Raylib.LoadImageFromTexture(renderTexture.texture);
                    TransmitImage(image);
                    Raylib.UnloadImage(image);
                }

                // always use begin/end drawing to allow closing the window
                Raylib.BeginDrawing();
                if (!simSettings.HeadlessMode)
                {
                    // debug render to window
                    Raylib.DrawTextureRec(renderTexture.texture, textureRec, texturePos, Raylib.Colors.White);
                }
                Raylib.EndDrawing();
            }

            Raylib.UnloadShader(shader);
            Raylib.CloseWindow();
        }

        private static byte[] IntPtrToByteArray(IntPtr ptr, int length)
        {
            var bytes = new byte[length];
            Marshal.Copy(ptr, bytes, 0, bytes.Length);
            return bytes;
        }

        private void TransmitImage(Raylib.Image image)
        {
            const int bytesPerPixel = 4; // RGBA
            var bitmap = IntPtrToByteArray(image.data, image.width * image.height * bytesPerPixel);
            settings.Channel.Write(bitmap);
        }
    }
}
