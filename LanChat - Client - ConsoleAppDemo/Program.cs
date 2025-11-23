using System;
using LanChat.Networking;
using System.Threading;

namespace LanChat_ConsoleAppDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ClientSocket client = new ClientSocket();
            Console.Write("Enter server IP: ");
            string ip = Console.ReadLine();
            Console.Write("Enter your nickname: ");
            string nickname = Console.ReadLine();
            client.OnMessageReceived += (message) =>
            {
                Console.WriteLine(message);
            };
            client.Connect(ip, 5000, nickname);
            Console.WriteLine("Type messages to send to the server. Type 'exit' to quit.");
            while (true)
            {
                string message = Console.ReadLine();
                if (message.ToLower() == "exit") break;
                client.SendMessage(message);
            }
        }
    }
}