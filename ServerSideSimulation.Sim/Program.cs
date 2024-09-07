using ServerSideSimulation.Sim.Platform.Raylib;

namespace ServerSideSimulation.Sim
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Raylib.InitWindow(800, 800, "Hello Raylib");
			Raylib.SetTargetFPS(60);

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

			while (!Raylib.WindowShouldClose())
			{
				Raylib.ClearBackground(Raylib.Colors.White);
				Raylib.BeginDrawing();

				angle += angleDelta;
				Raylib.DrawRectanglePro(rect, origin, angle, Raylib.Colors.Blue);

				Raylib.EndDrawing();
			}

			Raylib.CloseWindow();
		}
	}
}
