using AppKit;

namespace JARVIS.Client.Mac
{
    static class MainClass
    {
        public static Services.Socket.SocketClient Client = new Services.Socket.SocketClient();

        static void Main(string[] args)
        {
            NSApplication.Init();
            NSApplication.Main(args);
        }
    }
}
