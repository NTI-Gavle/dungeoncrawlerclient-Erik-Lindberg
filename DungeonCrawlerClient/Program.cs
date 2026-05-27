using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DungeonCrawlerClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAddress =
                IPAddress.Parse("127.0.0.1");

            IPEndPoint endPoint =
                new IPEndPoint(ipAddress, 54321);

            TcpClient tcpClient = new TcpClient();

            try
            {
                tcpClient.Connect(endPoint);

                Console.WriteLine("Connected.");
            }
            catch
            {
                Console.WriteLine("Could not connect.");
                return;
            }

            NetworkStream stream =
                tcpClient.GetStream();

            while (tcpClient.Connected)
            {
                try
                {
                    byte[] bytes = new byte[1024];

                    int length =
                        stream.Read(bytes, 0, bytes.Length);

                    if (length == 0)
                    {
                        break;
                    }

                    string message =
                        Encoding.UTF8.GetString(bytes, 0, length);

                    Console.Write(message);

                    string? command = Console.ReadLine();

                    if (command == null)
                    {
                        continue;
                    }

                    byte[] writeBytes =
                        Encoding.UTF8.GetBytes(command);

                    stream.Write(
                        writeBytes,
                        0,
                        writeBytes.Length);
                }
                catch
                {
                    Console.WriteLine("Disconnected.");
                    break;
                }
            }

            tcpClient.Close();
        }
    }
}