using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanChat.SQLServerDatabase
{
    internal class UserRepostory
    {
        public static bool UserExists(string username)
        {
            // Kiem tra neu nguoi dung ton tai trong co so du lieu
            using (SqlConnection connection = new SqlConnection(DataBaseHelper.LanChatDBConnection))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public static bool ValidateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(DataBaseHelper.LanChatDBConnection))
            {
                connection.Open();
                string query =
                    "SELECT COUNT(*) FROM Users WHERE Username = @u AND Password = @p";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@u", username);
                    command.Parameters.AddWithValue("@p", password);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public static bool RegisterUser(string username, string password)
        {
            if (UserExists(username)) return false;

            using (SqlConnection con = new SqlConnection(DataBaseHelper.LanChatDBConnection))
            {
                con.Open();
                string query = "INSERT INTO Users (Username, PasswordHash) VALUES (@u, @p)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", Hash(password));

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private static string Hash(string input)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
