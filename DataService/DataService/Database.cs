using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;

namespace DataService
{
    class Database
    {
        public static void router(string[] data, SqlConnection connection)
        {
            if (data[0] == "connect")
            {
                connect(data[1], connection);
            }

            else if (data[0] == "upload")
            {
                Console.WriteLine(data.Length);
                upload(data);
            }

            else if (data[0] == "download")
            {
                download(data[1]);
            }

            else if (data[0] == "health")
            {
                Communications.send_response("healthRply data", "localhost", 130);
            }

        }

        public static void connect(string id, SqlConnection connection)
        {
            string salt = DataProcessor.RandomString(255);
            string insert_id = $"SELECT Count(id) FROM [dbo].[passwords] WHERE id = '{id}'";

            SqlCommand cmd = new SqlCommand(insert_id, connection);
            string result = cmd.ExecuteScalar().ToString();

            if (result == "0")
            {
                string insertuserquery = $"INSERT INTO [dbo].[passwords] (id, storage, salt) VALUES ('{id}','1','{salt}')";
                SqlCommand insertcmd = new SqlCommand(insertuserquery, connection);
                insertcmd.ExecuteNonQuery();

                DataProcessor.initializeDataSet(id);
            }
        }

        public static SqlConnection connectDB()
        {
            SqlConnection connection = null;
            Thread thread = new Thread(() => {
                string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\akula\Documents\data.mdf; Integrated Security = True; Connect Timeout = 30";
                connection = new SqlConnection(connectionString);
                connection.Open();

            });
            thread.Start();
            thread.Join();
            return connection;
        }

        public static void upload(string[] data)
        {
            Random rnd = new Random();
            string filename = Crypto.GenerateRandomAlphanumericString(rnd.Next(50, 150));

            string location = "c:\\cloud\\" + data[1] + "\\" + filename +".wtf";
            
            using (StreamWriter writer = new StreamWriter(@location))
            {
                writer.WriteLine(data[2]);
                writer.WriteLine(data[3]);
                writer.WriteLine(data[4]);
                writer.Close();
            }
            
        }

        public static void download(string id)
        {
            string fileLocation = "c:\\cloud\\" + id + "\\";
            string[] fileList = Directory.GetFiles(fileLocation);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < fileList.Length; i++)
            {
                string[] data = File.ReadAllLines(fileList[i]);
                sb.Append(data[0] + " ");
                sb.Append(data[1] + " ");
                sb.Append(data[2] + " ");
            }

            string accounts = sb.ToString();
            Console.WriteLine(accounts);
            Communications.send_data("localhost", 130, "downloadservice" ,accounts);
        }

    }
}
