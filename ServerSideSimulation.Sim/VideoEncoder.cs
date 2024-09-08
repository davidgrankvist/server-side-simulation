using System.Diagnostics;

namespace ServerSideSimulation.Sim
{
    internal class VideoEncoder
    {
        private BitmapChannel channel;
        private int screenWidth;
        private int screenHeight;
        private int fps;
        private bool verbose;

        public VideoEncoder(BitmapChannel channel, int screenWidth, int screenHeight, int fps, bool verbose = false)
        {
            this.channel = channel;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.fps = fps;
            this.verbose = verbose;
        }

        public async Task Run()
        {
            await RunEncoding();
            //await OutputRawFrames();
            Console.WriteLine("Encoding job ended.");
        }

        // output bitmap data to for manual testing of the ffmpeg command
        private async Task OutputRawFrames()
        {
            var maxFrames = 100; // TODO(prototype): guard for when outputting to video
            var frameCount = 0;

            var outputFile = Path.Combine(AppContext.BaseDirectory, "input.raw");
            using (var fileStream = File.Create(outputFile))
            {
                await foreach (var frame in channel.ReadAllAsync())
                {
                    if (frameCount++ >= maxFrames)
                    {
                        Console.WriteLine($"Encoded the maximum {maxFrames} frames. Ending encode loop.");
                        break;
                    }

                    fileStream.Write(frame);
                }
            }
        }

        private async Task RunEncoding()
        {

            /*
             * Tested manually by outputting raw frames to input.raw and running the following:
             *
             * ffmpeg -f rawvideo -pix_fmt rgba -s 800x800 -r 60 -i input.raw -c:v libx264 -pix_fmt yuv420p -f mp4 output.mp4 -y
             */

            var outputFile = Path.Combine(AppContext.BaseDirectory, "output.mp4");
            var ffmpegProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-f rawvideo -pix_fmt rgba -s {screenWidth}x{screenHeight} -r {fps} -i - -c:v libx264 -pix_fmt yuv420p -f mp4 output.mp4 -y",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = verbose,
                    RedirectStandardError = verbose,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            ffmpegProcess.Start();

            var maxExitWaitMs = 2000;
            try
            {
                await EncodeFrames(ffmpegProcess);
                Console.WriteLine("Done encoding.");

                Console.WriteLine("Ending ffmpeg process");
                ExitAfterTimeout(ffmpegProcess, maxExitWaitMs);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception while encoding frames.", ex);
                await FinishEncoding(ffmpegProcess, maxExitWaitMs);
            }
        }

        private void ExitAfterTimeout(Process process, int timeoutMs)
        {
            if (!process.WaitForExit(timeoutMs))
            {
                process.Kill();
            }
        }

        private async Task FinishEncoding(Process ffmpegProcess, int maxExitWaitMs)
        {
            Console.WriteLine("Capturing ffmpeg output");
            var output = verbose ? await ffmpegProcess.StandardOutput.ReadToEndAsync() : string.Empty;
            var error = verbose ? await ffmpegProcess.StandardError.ReadToEndAsync() : string.Empty ;

            Console.WriteLine("Ending ffmpeg process");
            ExitAfterTimeout(ffmpegProcess, maxExitWaitMs);

            var exitCode = ffmpegProcess.ExitCode;
            Console.WriteLine("Output:");
            Console.WriteLine(output);
            Console.WriteLine("Error:");
            Console.WriteLine(error);
            Console.WriteLine($"Process exited with code {exitCode}");
        }

        private async Task EncodeFrames(Process ffmpegProcess)
        {
            var maxFrames = 100; // TODO(prototype): guard for when outputting to video
            var frameCount = 0;
            using (var stdin = ffmpegProcess.StandardInput.BaseStream)
            {
                await foreach (var frame in channel.ReadAllAsync())
                {
                    if (frameCount++ >= maxFrames)
                    {
                        Console.WriteLine($"Encoded the maximum {maxFrames} frames. Ending encode loop.");
                        break;
                    }

                    if (stdin.CanWrite)
                    {
                        await stdin.WriteAsync(frame);
                    }
                }
                stdin.Close();
            }
        }
    }
}
