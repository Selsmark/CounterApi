namespace SocketServer
{
    internal class Program
    {
        static int Main(string[] args)
        {
            SocketListener.StartServer();
            return 0;
        }
    }
}
