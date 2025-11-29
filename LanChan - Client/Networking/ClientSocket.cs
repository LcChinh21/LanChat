using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanChat.Networking
{
    internal class ClientSocket
    {
        // Khai báo các biến cần thiết
        // TcpClient đại diện cho kết nối TCP với server
        private TcpClient client;
        // NetworkStream để gửi và nhận dữ liệu
        private NetworkStream stream;
        // Thread là tạo 1 luồng chạy song song với luồng chính
        // Luồng này có thể chạy song song với UI
        private Thread listenThread;

        // Trạng thái client
        // { get; private set; } có nghĩa là được cho phép GET từ bên ngoài những chỉ được SET (PRIVATE SET) ở trong class
        public string NickName { get; private set; }

        public bool IsConnected { get; private set; } = false;

        // Tạo một sự kiện để thông báo khi có tin nhắn mới
        public event Action<string> OnMessageReceived;

        // Kết nối tới server
        public void Connect(string ip, int port, string nickname)
        {
            // try - catch để bắt lỗi khi kết nối
            // Khi có lỗi xảy ra trong khối try, chương trình sẽ nhảy vào khối catch để xử lý lỗi
            try
            {
                // Tạo một đối tượng TcpClient và kết nối tới server
                client = new TcpClient();
                client.Connect(ip, port);
                // Lấy stream để gửi và nhận dữ liệu
                stream = client.GetStream();
                // Cập nhật trạng thái kết nối
                IsConnected = true;
                // Lưu nickname
                NickName = nickname;
                SendMessage(nickname); // Gửi nickname lên server
                // Tạo và bắt đầu một luồng mới để lắng nghe tin nhắn từ server
                listenThread = new Thread(ListenForMessages);
                listenThread.IsBackground = true;
                listenThread.Start();
                MessageBox.Show("Connected to server.");
            }
            // Khi có lỗi xảy ra, hiểu thị thông báo lỗi
            // System.Exception là lớp cơ sở cho tất cả các ngoại lệ trong .NET
            // Nó chứa thông tin về lỗi đã xảy ra
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to server: {ex.Message}");
            }
        }
        // Gửi tin nhắn tới server
        public void SendMessage(string message)
        {
            // Nếu chưa kết nối thì không gửi được tin nhắn
            if (!IsConnected) return;

            // Chuyển đổi tin nhắn thành mảng byte và gửi qua stream
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
        }
        public void ListenForMessages()
        {
            // Tạo buffer để nhận dữ liệu
            byte[] buffer = new byte[1024];
            while (IsConnected)
            {
                try
                {
                    // Đọc dữ liệu từ stream
                    int byteCount = stream.Read(buffer, 0, buffer.Length);
                    if (byteCount == 0) break; // Server ngắt kết nối
                    string message = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    OnMessageReceived?.Invoke(message);
                }
                catch (Exception)
                {
                    // Nếu lỗi xảy ra thì ngắt kết nối
                    // Vì lỗi có thể do server ngắt kết nối hoặc sự cố mạng
                    // Nên ta sẽ ngắt kết nối client
                    Disconnect();
                    break;
                }
            }
        }
        public void Disconnect()
        {
            // Nếu chưa kết nối thì không cần ngắt kết nối
            if (!IsConnected) return;
            // Cập nhật trạng thái kết nối
            IsConnected = false;
            // Đóng stream và client
            stream.Close();
            client.Close();
            // Chờ luồng lắng nghe kết thúc
            listenThread.Join();
            Console.WriteLine("Disconnected from server.");
        }
    }
}
