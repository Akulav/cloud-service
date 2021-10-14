using System.Data.SqlClient;
using System.Threading;

namespace Gateway
{
    class Database
    {
        public static SqlConnection connectDB()
        {
            SqlConnection connection = null;
            Thread thread = new Thread(() => {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\akula\Documents\whitelist.mdf;Integrated Security=True;Connect Timeout=30";
                connection = new SqlConnection(connectionString);
                connection.Open();

            });
            thread.Start();
            thread.Join();
            return connection;
        }

        public static void insertServicePort(SqlConnection connection, string port,string service)
        {
            string insertquery = $"UPDATE [dbo].[Table] SET port = ('{port}') WHERE service = ('{service}')";
            SqlCommand cmd = new SqlCommand(insertquery, connection);
            cmd.ExecuteNonQuery();
        }

        public static void insertServiceIP(SqlConnection connection, string ip, string service)
        {
            string insertquery = $"UPDATE [dbo].[Table] SET ip = ('{ip}') WHERE service = ('{service}')";
            SqlCommand cmd = new SqlCommand(insertquery, connection);
            cmd.ExecuteNonQuery();
        }

        public static string[] getAddress(SqlConnection connection,string service)
        {
            string queryIP = $"SELECT ip FROM [dbo].[Table] WHERE service = '{service}'";
            string queryPORT = $"SELECT port FROM [dbo].[Table] WHERE service = '{service}'";
            SqlCommand cmd = new SqlCommand(queryIP, connection);
            SqlCommand cmd1 = new SqlCommand(queryPORT, connection);
            string ip = cmd.ExecuteScalar().ToString();
            string port = cmd1.ExecuteScalar().ToString();
            string data = ip + " " + port;
            string[] address = DataProcessor.wordArray(data.ToCharArray());
            return address;
        }

    }
}

