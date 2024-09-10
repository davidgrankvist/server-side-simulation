using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace ServerSideSimulation.Server
{
    internal class WebServer
    {
        private static readonly string baseUrl = "http://localhost:8080";

        public Task Run()
        {
            return Task.Run(StartListener);
        }

        private static async Task StartListener()
        {
            var httpListener = new HttpListener();
            var endPoint = baseUrl + "/";
            httpListener.Prefixes.Add(endPoint);
            httpListener.Start();
            Console.WriteLine($"Listening on {endPoint}");

            while (true)
            {
                var listenerContext = await httpListener.GetContextAsync();
                _ = Task.Run(() => HandleRequestAndErrors(listenerContext));
            }
        }

        private static async Task HandleRequestAndErrors(HttpListenerContext context)
        {
            try
            {
                await RouteRequest(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught error from handler", ex);
            }
        }

        private static async Task RouteRequest(HttpListenerContext context)
        {
            var url = context.Request.Url?.ToString() ?? "";
            var path = url.Length >= baseUrl.Length ? url.Remove(0, baseUrl.Length) : url;

            Console.WriteLine($"Routing request for path {path}");

            if (path.StartsWith("/ws"))
            {
                await HandleWebsocketRequest(context);
            }
            else if (path == "/" || path.StartsWith("/index.html"))
            {
                await HandleFileRequest(context, "/index.html");
            }
            else
            {
                Console.WriteLine("Route not found. Returning 404.");
                context.Response.StatusCode = 404;
                context.Response.Close();
            }
        }

        private static async Task HandleFileRequest(HttpListenerContext context, string path)
        {
            Console.WriteLine("Incoming file request");

            var root = new string[] { "wwwroot" };
            var pathSegments =  path.Split('/');
            var filePath = Path.Combine(root.Concat(pathSegments).ToArray());

            Console.WriteLine("Serving", filePath);

            if (!File.Exists(filePath))
            {
                context.Response.StatusCode = 404;
                context.Response.Close();
                return;
            }

            var fileBytes = await File.ReadAllBytesAsync(filePath);
            context.Response.ContentType = "text/html";
            context.Response.ContentLength64 = fileBytes.Length;

            await context.Response.OutputStream.WriteAsync(fileBytes, 0, fileBytes.Length);
        }

        private static async Task HandleWebsocketRequest(HttpListenerContext context)
        {
            Console.WriteLine("Incoming ws request.");

            if (!context.Request.IsWebSocketRequest)
            {
                Console.WriteLine("Received a non-websocket request.");
                context.Response.StatusCode = 400;
                context.Response.Close();

                return;
            }

            Console.WriteLine("Received a websocket request. Accepting.");

            var wsContext = await context.AcceptWebSocketAsync(null);
            var ws = wsContext.WebSocket;

            var cancelSrc = new CancellationTokenSource();

            var sendTask = Task.Run(() => RunSendLoop(ws, cancelSrc.Token));
            var receiveTask = Task.Run(() => RunReceiveLoop(ws, cancelSrc.Token));
            var completed = await Task.WhenAny(sendTask, receiveTask);

            cancelSrc.Cancel();

            await Task.WhenAll(sendTask, receiveTask);
        }

        // TODO(incomplete): dummy implementation
        private static async Task RunSendLoop(WebSocket ws, CancellationToken cancellation)
        {
            Console.WriteLine("Starting send message loop.");
            while (ws.State == WebSocketState.Open && !cancellation.IsCancellationRequested)
            {
                var message = "Hello, this is the date: " + DateTime.Now;
                var messageBytes = Encoding.UTF8.GetBytes(message);

                await ws.SendAsync(messageBytes, WebSocketMessageType.Text, true, cancellation);

                await Task.Delay(1000);
            }
            Console.WriteLine("Ended send message loop");
        }

        // receive messages to check if the client sent a close message
        private static async Task RunReceiveLoop(WebSocket ws, CancellationToken cancellation)
        {
            Console.WriteLine("Starting receive message loop.");

            var buffer = new byte[128]; // small buffer to be able to receive close message
            while (ws.State == WebSocketState.Open && !cancellation.IsCancellationRequested)
            {
                var result = await ws.ReceiveAsync(buffer, cancellation);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("Received close message.");
                    break;
                }
            }
            Console.WriteLine("Ended receive message loop.");
        }
    }
}
