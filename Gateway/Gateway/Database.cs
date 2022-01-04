using System.Data.SqlClient;
using System.Threading;

namespace Gateway
{
    class Database
    {
        public static SqlConnection connectDB()
        {
            SqlConnection connection = null;
            Thread thread = new Thread(() =>
            {
                
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\akula\Documents\whitelist.mdf;Integrated Security=True;Connect Timeout=30";
                connection = new SqlConnection(connectionString);
                connection.Open();

            });
            thread.Start();
            thread.Join();
            return connection;
        }

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

 

        public static void insertServiceERROR(SqlConnection connection, string service)
        {
            string queryFails = $"SELECT fails FROM [dbo].[Table] WHERE service = '{service}'";
            SqlCommand cmd1 = new SqlCommand(queryFails, connection);
            int fails = int.Parse(cmd1.ExecuteScalar().ToString());

            if (fails < 3)
            {
                string insertquery = $"UPDATE [dbo].[Table] SET fails = ('{fails + 1}') WHERE service = ('{service}')";
                SqlCommand cmd = new SqlCommand(insertquery, connection);
                cmd.ExecuteNonQuery();
            }

            else
            {
                string dropquery = $"UPDATE [dbo].[Table] SET fails = ('0') WHERE service = ('{service}') UPDATE [dbo].[Table] SET ip = null WHERE service = ('{service}') UPDATE [dbo].[Table] SET port = null WHERE service = ('{service}')";
                SqlCommand cmd2 = new SqlCommand(dropquery, connection);
                cmd2.ExecuteNonQuery();
            }
        }

        public static string[] getAddress(SqlConnection connection, string service)
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

