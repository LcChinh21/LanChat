using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanChat___Server
{
    internal class Message
    {
        // Loại tin nhắn: message / system / login / private / online
        public string Type { get; set; }

        // Người gửi
        public string From { get; set; }

        // Người nhận (private message)
        public string To { get; set; }

        // Nội dung
        public string Text { get; set; }

        // Danh sách user (nếu Type = online)
        public string[] Users { get; set; }

        // Thời gian gửi (ISO 8601)
        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("o");

        // -----------------------------
        // Serialize → string
        // -----------------------------
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        // -----------------------------
        // Deserialize ← string
        // -----------------------------
        public static Message FromJson(string json)
        {
            return JsonSerializer.Deserialize<Message>(json);
        }

        // -----------------------------
        // Tạo tin nhắn hệ thống
        // -----------------------------
        public static Message System(string text)
        {
            return new Message
            {
                Type = "system",
                Text = text
            };
        }

        // -----------------------------
        // Tạo tin nhắn chat thường
        // -----------------------------
        public static Message Chat(string from, string text)
        {
            return new Message
            {
                Type = "message",
                From = from,
                Text = text
            };
        }

        // -----------------------------
        // Tạo tin nhắn private
        // -----------------------------
        public static Message Private(string from, string to, string text)
        {
            return new Message
            {
                Type = "private",
                From = from,
                To = to,
                Text = text
            };
        }

        // -----------------------------
        // Tạo tin nhắn danh sách online
        // -----------------------------
        public static Message Online(string[] users)
        {
            return new Message
            {
                Type = "online",
                Users = users
            };
        }
    }
}
