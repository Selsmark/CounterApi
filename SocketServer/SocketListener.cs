using RSAKeyExchangeModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CounterServer
{
    public class SocketListener
    {
        private static readonly string PublicPemFilename = "public.pem";
        private static readonly string PrivatePemFilename = "private.pem";

        public static void StartServer()
        {
            const string ip = "192.168.100.52";
            const int port = 11000;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            try
            {
                IPAddress localAddress = IPAddress.Parse(ip);
                //server = new TcpListener(localAddress, port);
                IPEndPoint localEndPoint = new IPEndPoint(localAddress, port);

                // Create a TCP socket
                Socket serverSocket = new Socket(localAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(localEndPoint);

                // Listen for incoming connections
                serverSocket.Listen(10);
                Console.WriteLine("Server listening on port 11000");

                // Accept incoming connections
                while (true)
                {
                    Socket clientSocket = serverSocket.Accept();

                    // Handle the connection in a separate thread
                    Thread thread = new Thread(() => HandleClient(clientSocket));
                    thread.Start();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void HandleClient(Socket clientSocket)
        {
            try
            {
                while (clientSocket.Connected)
                {
                    // Receive data from the client
                    byte[] buffer = new byte[2048];
                    int bytesReceived = clientSocket.Receive(buffer);

                    // Check if the client has closed the connection
                    if (bytesReceived == 0)
                    {
                        Console.WriteLine("Client disconnected");
                        break;
                    }

                    // Convert the received data to a string and return a encrypted API token
                    var receivedPublicKey = Encoding.UTF8.GetString(buffer, 0, bytesReceived);

                    string testToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjBiYTUwNjcxLTQ5N2QtNDJjZS05NGE3LWNlY2M4ZDNmMjg0NSIsInN1YiI6Im5pY2tsYXNAc2Vsc21hcmsuZGsiLCJ";

                    byte[] encryptedToken = Cryptor.EncryptData(testToken, receivedPublicKey);

                    Console.WriteLine(encryptedToken);

                    buffer = encryptedToken;
                    clientSocket.Send(buffer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally { clientSocket.Close(); }
        }

    }
}
