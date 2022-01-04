using System.Data.SqlClient;
using System.Threading;

namespace Gateway
{
    class Database
    {

        public static SqlConnection getDB(string service)
        {
            SqlConnection connection = null;
            if (service == "user")
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\akula\Documents\logins.mdf;Integrated Security=True;Connect Timeout=30";
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            else if (service == "logger")
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\akula\Documents\loggers.mdf;Integrated Security=True;Connect Timeout=30";
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            else if (service == "data")
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\akula\Documents\datas.mdf;Integrated Security=True;Connect Timeout=30";
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            else if (service == "cache")
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\akula\Documents\caches.mdf;Integrated Security=True;Connect Timeout=30";
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            return connection;
        }    


        public static void insertData(SqlConnection connection, string id, string ip)
        {
            try
            {
                string insertquery = $"INSERT INTO [dbo].[table](id, errors, ip) VALUES('{id}', '0', '{ip}')";
                SqlCommand cmd = new SqlCommand(insertquery, connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

    }
}

