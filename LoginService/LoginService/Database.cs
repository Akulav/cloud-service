using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LoginService
{
    class Database
    {
        public static void login(string username, string password)
        {
            Random rnd = new Random();
            var id = rnd.Next(20, 200);
            string query = $"INSERT INTO [dbo].[Data] (id, username, password) VALUES ('{id}','{username}','{password}')";

            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\akula\Documents\users.mdf; Integrated Security = True; Connect Timeout = 30";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.ExecuteNonQuery();
        }

        public static void signup(string username, string password)
        {

        }
    }
}
