using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanChat.SQLServerDatabase
{
    // Tao class DataBaseHelper de xu ly ket noi va thao tac voi co so du lieu SQL Server
    internal class DataBaseHelper
    {
        public static string MasterConnection = "Server=localhost;Database=master;User Id=sa;Password=your_password;";
        public static string LanChatDBConnection = "Server=localhost;Database=LanChatDB;User Id=sa;Password=your_password;";

        public static void Initialize()
        {
            CreateDatabase();
        }
        
        private static void CreateDatabase()
        {
            try
            {
                using (var connection = new System.Data.SqlClient.SqlConnection(MasterConnection))
                {
                    connection.Open();
                    string createDbQuery = "IF DB_ID('LanChatDB') IS NULL CREATE DATABASE LanChatDB;";
                    using (var command = new System.Data.SqlClient.SqlCommand(createDbQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo cơ sở dữ liệu: " + ex.Message);
            }
        }
    }
}
