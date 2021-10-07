using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DataService
{
    class Database
    {
        public static void router(string[] data)
        {
            if (data[0] == "connect")
            {
                connect(data[1]);
            }

        }

        public static void connect(string id)
        {
            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\akula\Documents\data.mdf; Integrated Security = True; Connect Timeout = 30";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string salt = DataProcessor.RandomString(255);
            string insert_id = $"SELECT Count(id) FROM [dbo].[passwords] WHERE id = '{id}'";

            SqlCommand cmd = new SqlCommand(insert_id, connection);
            string result = cmd.ExecuteScalar().ToString();

            if (result == "0")
            {
                string insertuserquery = $"INSERT INTO [dbo].[passwords] (id, storage, salt) VALUES ('{id}','1','{salt}')";
                SqlCommand insertcmd = new SqlCommand(insertuserquery, connection);
                insertcmd.ExecuteNonQuery();
            }
        }
    }
}
