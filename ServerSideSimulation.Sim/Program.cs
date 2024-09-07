using ServerSideSimulation.Sim.Platform.Raylib;

namespace ServerSideSimulation.Sim
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var screenWidth = 800;
			var screenHeight = 800;
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

			Raylib.InitWindow(screenWidth, screenHeight, "Hello Raylib");
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

				// encode/stream the video here

				Raylib.UnloadImage(image);

				// debug rendering to window
				Raylib.BeginDrawing();
				Raylib.ClearBackground(Raylib.Colors.White);
				Raylib.DrawTextureRec(renderTexture.texture, textureRec, texturePos, Raylib.Colors.White);
				Raylib.EndDrawing();
			}

			Raylib.CloseWindow();
		}
	}
}
