using System;
using Community.CsharpSqlite.SQLiteClient;
using System.IO;
using System.Text;

namespace DataService
{
    class Database
    {
        public static void router(string[] data, SqliteConnection connection)
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
        }

        public static void CreateDB()
        {

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\databases"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\databases");

               
                    var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + "data.mdf");
                    var con = $@"URI=file:{dbName}";
                    File.WriteAllText(dbName, null);
                    var connection = new SqliteConnection(con);
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                    cmd.CommandText = @"CREATE TABLE data(id VARCHAR(250), storage VARCRHAR(250), salt VARCRHAR(250))";
                    cmd.ExecuteNonQuery();
                };
                    
            }
        }

        public static void connect(string id, SqliteConnection connection)
        {
            
            string salt = DataProcessor.RandomString(255);
            string insert_id = $"SELECT Count(id) FROM data WHERE id = '{id}'";

            SqliteCommand cmd = new SqliteCommand(insert_id, connection);
            string result = cmd.ExecuteScalar().ToString();
            Console.WriteLine(result);

            if (result == "0")
            {
                string insertuserquery = $"INSERT INTO data (id, storage, salt) VALUES ('{id}','1','{salt}')";
                SqliteCommand insertcmd = new SqliteCommand(insertuserquery, connection);
                insertcmd.ExecuteNonQuery();

                DataProcessor.initializeDataSet(id);
            }
        }

        public static SqliteConnection connectDB()
        {
            CreateDB();
            var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + "data.mdf");
            string connectionString = $@"URI=file:{dbName}";SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            return connection;
        }

        public static void upload(string[] data)
        {
            Random rnd = new Random();
            string filename = Crypto.GenerateRandomAlphanumericString(rnd.Next(50, 150));

            string location = (Directory.GetCurrentDirectory()+ "\\" + data[1] + "\\" + filename +".wtf");
            
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
            string fileLocation = (Directory.GetCurrentDirectory() + "\\" + id + "\\");
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
