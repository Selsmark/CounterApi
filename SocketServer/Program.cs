namespace CounterServer
{
    internal class Program
    {
        static int Main(string[] args)
        {
            SocketListener.StartServer();
            return 0; // Everything went fine
        }
    }
}
