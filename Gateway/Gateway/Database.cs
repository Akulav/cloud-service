using System;
using System.Data.SqlClient;
using System.IO;

namespace Gateway
{
    class Database
    {

        public static SqlConnection getDB(string service)
        {
            SqlConnection connection = null;
            if (service == "user")
            {
                string connectionString = $@"AttachDbFilename={Directory.GetCurrentDirectory()+"\\logins.mdf"};Integrated Security=True;Connect Timeout=30";
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            else if (service == "data")
            {
                string connectionString = $@"AttachDbFilename={Directory.GetCurrentDirectory() + "\\datas.mdf"};Integrated Security=True;Connect Timeout=30";
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            else if (service == "cache")
            {
                string connectionString = $@"AttachDbFilename={Directory.GetCurrentDirectory() + "\\caches.mdf"};Integrated Security=True;Connect Timeout=30";
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            return connection;
        }    

        public static void processRequest(string service, string data_string, bool client)
        {
            SqlConnection connection = getDB(service);
            string query = @"SELECT * FROM [dbo].[Table]";
            SqlCommand cmd = new SqlCommand(query, connection);
            var i = 1;
        read:
            var Table = cmd.ExecuteReader();

            while (true)
            {
                try
                {
                    for (int j = 0; j < i; j++)
                    {
                        Table.Read();
                    }
                    i++;
                    Communication.send_response(data_string, Table[2].ToString(), Table[0].ToString(),  connection);

                    if (client)
                    {
                        Communication.send_to_client(data_string);
                    }

                    break;
                }
                catch
                {
                    Console.WriteLine("ERROR");
                    Console.WriteLine(i);
                    string id = Table[0].ToString();
                    int fails = int.Parse(Table[1].ToString());
                    Table.Close();

                    if (fails < 3)
                    {
                        string insertquery = $"UPDATE [dbo].[Table] SET errors = ('{fails + 1}') WHERE id = ('{id}')";
                        SqlCommand cmd3 = new SqlCommand(insertquery, connection);
                        cmd3.ExecuteNonQuery();
                    }

                    else
                    {
                        string dropquery = $"DELETE FROM [dbo].[Table] WHERE id = '{id}'";
                        SqlCommand cmd2 = new SqlCommand(dropquery, connection);
                        cmd2.ExecuteNonQuery();
                        i--;
                    }
                    goto read;
                }
            }
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

