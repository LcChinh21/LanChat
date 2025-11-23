using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LanChat.Networking
{
    internal class ServerSocket
    {
        private TcpListener server;
        private Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();

        public bool IsRunning { get; private set; } = false;

        public void Start(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            Console.WriteLine($"Server started on port {port}");
            IsRunning = true;

            Thread listenThread = new Thread(ListenForClients);
            listenThread.Start();
        }

        private void ListenForClients()
        {
            Console.WriteLine("Waiting for clients...");

            while (IsRunning)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            // Nhận nickname từ client
            int byteCount = stream.Read(buffer, 0, buffer.Length);
            string nickname = Encoding.UTF8.GetString(buffer, 0, byteCount);

            Console.WriteLine($"{nickname} connected.");

            lock (clients)
            {
                clients[nickname] = client;
            }

            BroadcastOnlineList();

            while (true)
            {
                try
                {
                    byteCount = stream.Read(buffer, 0, buffer.Length);
                    if (byteCount == 0) break; // client ngắt kết nối

                    string msg = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    Console.WriteLine($"[{nickname}]: {msg}");
                    BroadcastMessage($"[{nickname}]: {msg}");
                }
                catch
                {
                    break;
                }
            }

            Console.WriteLine($"{nickname} disconnected.");

            lock (clients)
            {
                clients.Remove(nickname);
            }

            BroadcastOnlineList();
            client.Close();
        }

        private void BroadcastMessage(string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            lock (clients)
            {
                foreach (var c in clients.Values)
                {
                    c.GetStream().Write(data, 0, data.Length);
                }
            }
        }

        private void BroadcastOnlineList()
        {
            string listMsg = "online status:" + string.Join(",", clients.Keys);
            byte[] data = Encoding.UTF8.GetBytes(listMsg);

            lock (clients)
            {
                foreach (var c in clients.Values)
                {
                    c.GetStream().Write(data, 0, data.Length);
                }
            }
        }
    }
}
