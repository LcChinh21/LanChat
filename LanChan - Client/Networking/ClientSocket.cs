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
        private TcpClient client;
        private NetworkStream stream;
        private Thread listenThread;

        public string NickName { get; private set; }
        public bool IsConnected { get; private set; } = false;

        public event Action<string> OnMessageReceived;

        public void Connect(string ip, int port, string nickname)
        {
            try
            {
                client = new TcpClient();
                client.Connect(ip, port);
                stream = client.GetStream();
                IsConnected = true;
                NickName = nickname;
                SendMessage(nickname); // Gửi nickname lên server
                listenThread = new Thread(ListenForMessages);
                listenThread.IsBackground = true;
                listenThread.Start();
                MessageBox.Show("Connected to server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to server: {ex.Message}");
            }
        }
        public void SendMessage(string message)
        {
            if (!IsConnected) return;

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
        }
        public void ListenForMessages()
        {
            byte[] buffer = new byte[1024];
            while (IsConnected)
            {
                try
                {
                    int byteCount = stream.Read(buffer, 0, buffer.Length);
                    if (byteCount == 0) break; // Server ngắt kết nối
                    string message = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    OnMessageReceived?.Invoke(message);
                }
                catch (Exception)
                {
                    Disconnect();
                    break;
                }
            }
        }
        public void Disconnect()
        {
            if (!IsConnected) return;
            IsConnected = false;
            stream.Close();
            client.Close();
            listenThread.Join();
            Console.WriteLine("Disconnected from server.");
        }
    }
}
