using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace Gateway
{
    class Database
    {
        public static SqlConnection connectDB()
        {
            SqlConnection connection = null;
            Thread thread = new Thread(() => {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\akula\Documents\cache.mdf;Integrated Security=True;Connect Timeout=30";
                connection = new SqlConnection(connectionString);
                connection.Open();

            });
            thread.Start();
            thread.Join();
            return connection;
        }

        public static void insertCacheHash(SqlConnection connection, string hash)
        {
            string inserthashquery = $"INSERT INTO [dbo].[Table] (hash) VALUES ('{hash}')";
            SqlCommand cmd = new SqlCommand(inserthashquery, connection);
            cmd.ExecuteNonQuery();
        }

        public static void insertCacheReply(SqlConnection connection, string reply, string hash)
        {
            string insertreplyquery = $"UPDATE [dbo].[Table] SET reply = ('{reply}') WHERE hash = ('{hash}')";
            SqlCommand cmd = new SqlCommand(insertreplyquery, connection);
            cmd.ExecuteNonQuery();
        }

        public static string checkHash(SqlConnection connection, string hash)
        {
            string checkquery = $"SELECT COUNT(hash)FROM [dbo].[Table] WHERE hash = '{hash}'";
            string getreplyquery = $"SELECT reply FROM [dbo].[Table] WHERE hash = '{hash}'";

            SqlCommand cmd = new SqlCommand(checkquery, connection);
            string result = cmd.ExecuteScalar().ToString();

            if (result == "1")
            {
                SqlCommand replyget = new SqlCommand(getreplyquery, connection);
                string reply = replyget.ExecuteScalar().ToString();
                Console.WriteLine(reply);
                return reply;
            }

            else
            {
                return null;
            }
        }
    }
}
