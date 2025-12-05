using LanChat.SQLServerDatabase;
using LanChat___Server;
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
        // Khai báo các biến cần thiết
        private TcpListener server;
        // Lưu trữ các client đã kết nối
        private Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();
        // Trạng thái server
        // { get; private set; } có nghĩa là được cho phép GET từ bên ngoài những chỉ được SET (PRIVATE SET) ở trong class
        public bool IsRunning { get; private set; } = false;

        // Phương thức khởi động server
        public void Start(int port)
        {
            // Khởi tạo TcpListener và bắt đầu lắng nghe kết nối
            // IPAddress.Any nghĩa là lắng nghe trên tất cả các địa chỉ IP của máy chủ
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            // Hiển thị thông báo server đã khởi động
            Console.WriteLine($"Server started on port {port}");
            IsRunning = true;

            // Khoi tao Database
            SQLServerDatabase.DataBaseHelper.Initialize();

            Console.WriteLine("Database initialized.");

            // Thread là tạo luồn chạy song song với luồng chính
            // Tạo một luồng mới để lắng nghe các kết nối từ client
            Thread listenThread = new Thread(ListenForClients);
            // Bắt đầu luồng lắng nghe
            listenThread.Start();

        }

        private void ListenForClients()
        {
            // Hiển thị thông báo chờ kết nối từ client
            Console.WriteLine("Waiting for clients...");

            // Chờ và chấp nhận các kết nối từ client
            while (IsRunning)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }

        private void HandleClient(TcpClient client)
        {
            // Lấy stream để giao tiếp với client
            NetworkStream stream = client.GetStream();
            // Tạo buffer để nhận dữ liệu
            // Buffer là một mảng byte dùng để lưu trữ dữ liệu tạm thời khi truyền nhận
            byte[] buffer = new byte[1024];

            // Nhận nickname từ client
            int byteCount = stream.Read(buffer, 0, buffer.Length);
            string nickname = Encoding.UTF8.GetString(buffer, 0, byteCount);

            Console.WriteLine($"{nickname} connected.");

            // Lock để tránh xung đột khi nhiều client cùng kết nối
            // Lock chỉ cho phép một luồng truy cập vào đoạn mã bên trong cùng một thời điểm
            lock (clients)
            {
                clients[nickname] = client;
            }

            // Gửi danh sách online cho tất cả client
            BroadcastOnlineList();

            // Lắng nghe tin nhắn từ client
            while (true)
            {
                // try - catch để xử lý lỗi khi đọc dữ liệu
                // Khi dòng code trong try gặp lỗi, nó sẽ chuyển sang catch để xử lý thay vì làm sập chương trình
                try
                {
                    byteCount = stream.Read(buffer, 0, buffer.Length);
                    if (byteCount == 0) break; // client ngắt kết nối

                    // Chuyển đổi dữ liệu nhận được thành chuỗi để hiển thị
                    Message msg = Message.FromJson(Encoding.UTF8.GetString(buffer, 0, byteCount));
                    Console.WriteLine($"[{nickname}]: {msg}");
                    SendMessage(msg);
                }
                catch
                {
                    break;
                }
            }

            Console.WriteLine($"{nickname} disconnected.");

            lock (clients)
            {
                // Xóa client khỏi danh sách khi ngắt kết nối
                clients.Remove(nickname);
            }

            // Khi client ngắt kết nối, gửi thông báo cho tất cả client khác
            // Cập nhật lại danh sách online
            BroadcastOnlineList();

            // Đóng kết nối với client
            client.Close();
        }

        private void SendMessage(Message msg)
        {
            // Check message type
            if (msg.Type == "private")
            {
                // Gửi tin nhắn riêng tư
                if (clients.ContainsKey(msg.To))
                {
                    TcpClient toClient = clients[msg.To];
                    NetworkStream toStream = toClient.GetStream();
                    byte[] toBuffer = Encoding.UTF8.GetBytes(msg.ToJson());
                    toStream.Write(toBuffer, 0, toBuffer.Length);
                }
            }
            else if (msg.Type == "message")
            {
                // Gửi tin nhắn đến tất cả client
                byte[] buffer = Encoding.UTF8.GetBytes(msg.ToJson());
                lock (clients)
                {
                    foreach (var client in clients.Values)
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            else if (msg.Type == "system")
            {
                // Gửi tin nhắn hệ thống đến tất cả client
                byte[] buffer = Encoding.UTF8.GetBytes(msg.ToJson());
                lock (clients)
                {
                    foreach (var client in clients.Values)
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            else if (msg.Type == "login")
            {
                // Kiem tra username va password neu Login
                // Get username va password tu msg.Text (dua tren dinh dang "username:password")
                // Them du lieu vao DataBase neu Register
                // Get username va password tu msg.Text (dua tren dinh dang "username:password")
                // Xu li msg.Text
                byte[] buffer = Encoding.UTF8.GetBytes(msg.ToJson());
                string[] parts = msg.Text.Split(':');
                if (parts.Length != 2)
                {
                    // Gui thong bao loi ve client
                    Message errorMsg = Message.System("Invalid login format. Use username:password");
                    byte[] errorBuffer = Encoding.UTF8.GetBytes(errorMsg.ToJson());
                    lock (clients)
                    {
                        foreach (var client in clients.Values)
                        {
                            NetworkStream stream = client.GetStream();
                            stream.Write(errorBuffer, 0, errorBuffer.Length);
                        }
                    }
                    return;
                }
                if(UserRepostory.UserExists(parts[0]))
                {
                    // Kiem tra password
                    if(!UserRepostory.ValidateUser(parts[0], parts[1]))
                    {
                        // Gui thong bao loi ve client
                        Message errorMsg = Message.System("Invalid password.");
                        byte[] errorBuffer = Encoding.UTF8.GetBytes(errorMsg.ToJson());
                        lock (clients)
                        {
                            foreach (var client in clients.Values)
                            {
                                NetworkStream stream = client.GetStream();
                                stream.Write(errorBuffer, 0, errorBuffer.Length);
                            }
                        }
                        return;
                    }
                }
            }

        }

        // Khi 1 client kết nối hoặc ngắt kết nối, gửi danh sách user online đến tất cả client
        private void BroadcastOnlineList()
        {
            Message onlineMsg = new Message
            {
                Type = "online",
                Users = new string[clients.Count]
            };
            int index = 0;
            lock (clients)
            {
                foreach (var nickname in clients.Keys)
                {
                    onlineMsg.Users[index++] = nickname;
                }
            }
            SendMessage(onlineMsg);
        }
    }
}
