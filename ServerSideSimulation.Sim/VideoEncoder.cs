using System.Diagnostics;

namespace ServerSideSimulation.Sim
{
    internal class VideoEncoder
    {
        private readonly RenderSettings settings;
        private bool verbose;

        private bool frameCountGuard;
        private int maxFrames = 100;

        public VideoEncoder(RenderSettings settings, bool verbose = false)
        {
            this.settings = settings;
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
            frameCountGuard = true;
            var frameCount = 0;

            var outputFile = Path.Combine(AppContext.BaseDirectory, "input.raw");
            using (var fileStream = File.Create(outputFile))
            {
                await foreach (var frame in settings.Channel.ReadAllAsync())
                {
                    if (frameCountGuard && frameCount++ >= maxFrames)
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

            //var ffmpegProcess = CreateOutputToMp4Process();
            var ffmpegProcess = CreateOutputToUdpProcess();

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

        // publish the encoded video stream on UDP
        // TODO(investigate): the frames are published and can be decoded, but the result is partially corrupted
        private Process CreateOutputToUdpProcess()
        {
            /*
             * To manually capture the video stream an create an mp4, run the following command:
             *
             * ffmpeg -f mpegts -i udp://127.0.0.1:12345 -s 800x800 -r 60 -c:v libx264 -pix_fmt yuv420p -f mp4 output.mp4
             *
             * To create a dummy output stream to UDP, run
             *
             * ffmpeg -f lavfi -i testsrc=size=800x800:rate=60 -c:v libx264 -pix_fmt yuv420p -f mpegts udp://127.0.0.1:12345
             *
             * To output captured raw bitmap data, run
             *
             * ffmpeg -f rawvideo -pix_fmt rgba -s 800x800 -r 60 -i input.raw -c:v libx264 -pix_fmt yuv420p -f mpegts udp://127.0.0.1:12345
             *
             * To render the result in a window by using SDL, run
             *
             * Scripts\render_udp_video.bat
             */
            frameCountGuard = false;
            return CreateFfMpegProcess($"-f rawvideo -pix_fmt rgba -s {settings.ScreenWidth}x{settings.ScreenHeight} -r {settings.Fps} -i - -c:v libx264 -pix_fmt yuv420p -f mpegts udp://127.0.0.1:12345");
            //return CreateFfMpegProcess("-f lavfi -i testsrc=size=800x800:rate=60 -c:v libx264 -pix_fmt yuv420p -f mpegts udp://127.0.0.1:12345");
        }

        // output to mp4 file for manual testing
        private Process CreateOutputToMp4Process()
        {
            /*
             * Tested manually by outputting raw frames to input.raw and running the following:
             *
             * ffmpeg -f rawvideo -pix_fmt rgba -s 800x800 -r 60 -i input.raw -c:v libx264 -pix_fmt yuv420p -f mp4 output.mp4 -y
             */
            frameCountGuard = true;
            return CreateFfMpegProcess($"-f rawvideo -pix_fmt rgba -s {settings.ScreenWidth}x{settings.ScreenHeight} -r {settings.Fps} -i - -c:v libx264 -pix_fmt yuv420p -f mp4 output.mp4 -y");
        }

        private Process CreateFfMpegProcess(string args)
        {
            var outputFile = Path.Combine(AppContext.BaseDirectory, "output.mp4");
            var ffmpegProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = args,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = verbose,
                    RedirectStandardError = verbose,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            return ffmpegProcess;
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
            var frameCount = 0;
            using (var stdin = ffmpegProcess.StandardInput.BaseStream)
            {
                await foreach (var frame in settings.Channel.ReadAllAsync())
                {
                    if (frameCountGuard && frameCount++ >= maxFrames)
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
