using System;
using System.Collections.Generic; // Dùng Dictionary lưu trữ danh sách client
using System.Net;
using System.Net.Sockets; // Dùng TCP/IP
using System.Text; 
using System.Threading; // Luồng riêng cho mỗi client

namespace LanChat.Networking
{
    internal class ServerSocket
    {
        private TcpListener server;
        private Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>(); //key: nickname, value: địa chỉ IP  TcpClient

        public bool IsRunning { get; private set; } = false; // Kiểm tra server có đang chạy không

        public void Start(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start(); // Bắt đầu lắng nghe kết nối

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

        private void HandleClient(TcpClient client) // Nhận thông tin từ client rồi xử lý
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024]; // Dùng byte bởi vì socket truyền dữ liệu dạng byte - buffer là bộ đệm tạm thời lưu trữ thông tin client 

            // Nhận nickname từ client
            int byteCount = stream.Read(buffer, 0, buffer.Length);
            string nickname = Encoding.UTF8.GetString(buffer, 0, byteCount);

            Console.WriteLine($"{nickname} connected.");

            lock (clients) //tránh ghi đè hoặc mất dữ liệu và đảm bảo server ổn định khi nhiều client kết nối cùng lúc
            {
                clients[nickname] = client;
            }

            BroadcastOnlineList();

            while (true)
            {
                try
                {
                    byteCount = stream.Read(buffer, 0, buffer.Length);
                    if (byteCount == 0) break; // msg rỗng, client ngắt kết nối

                    string msg = Encoding.UTF8.GetString(buffer, 0, byteCount); // Giải mã msg từ byte sang string
                    Console.WriteLine(msg); //Viet ra msg nhận được từ client
                    BroadcastMessage(msg); // Gửi msg đến tất cả client khác
                }
                catch
                {
                    break;
                }
            }

            Console.WriteLine($"{nickname} disconnected.");
            // Thong bao client ngat ket noi

            lock (clients)
            {
                clients.Remove(nickname);
                // Ngat ket noi, thoat khoi vong lap va xoa client khoi danh sach
            }

            BroadcastOnlineList();
            // Cap nhat danh sach online
            client.Close();
            // Dong ket noi
        }

        private void BroadcastMessage(string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            // Chuyen msg tu string sang byte
            lock (clients)
            {
                foreach (var c in clients.Values)
                {
                    c.GetStream().Write(data, 0, data.Length);
                    // Gui msg den tat ca client
                }
            }
        }

        private void BroadcastOnlineList()
        {
            string listMsg = "online status:" + string.Join(",", clients.Keys);
            // Tao msg danh sach online
            byte[] data = Encoding.UTF8.GetBytes(listMsg);
            // Chuyen msg tu string sang byte
            lock (clients)
            {
                foreach (var c in clients.Values)
                {
                    c.GetStream().Write(data, 0, data.Length);
                    // Gui msg den tat ca client
                }
            }
        }
    }
}
