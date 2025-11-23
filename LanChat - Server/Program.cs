using System;
using LanChat.Networking;

namespace LanChat_Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServerSocket server = new ServerSocket();
            server.Start(5000); 
            Console.WriteLine("Press ENTER to stop the server...");
            Console.ReadLine();
        }
    }
}