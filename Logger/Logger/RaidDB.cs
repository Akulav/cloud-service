using System;
using System.Data.SQLite;
using System.IO;

namespace Cache
{
    internal class RaidDB
    {
        public static void createDB(int n)
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\databases"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\databases");

                for (int i = 0; i < n; i++)
                {
                    var dbName = (Directory.GetCurrentDirectory() + "\\databases" + "\\" + GenerateName());
                    var con = $@"URI=file:{dbName}";
                    File.WriteAllText(dbName, null);
                    var connection = new SQLiteConnection(con);
                    connection.Open();
                    var cmd = new SQLiteCommand(connection)
                    {
                        CommandText = @"CREATE TABLE user(id INTEGER PRIMARY KEY, hash VARCRHAR(250), command VARCRHAR(250))"
                    };
                    cmd.ExecuteNonQuery();
                }
            }          
        }

        public static string GenerateName()
        {
            var myUniqueFileName = string.Format(@"{0}.mdf", DateTime.Now.Ticks);
            return myUniqueFileName;
        }

        public static void writeData(string hash, string data)
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\databases", "*.mdf", SearchOption.AllDirectories);

            for (int i = 0;i < files.Length; i++)
            {
                var con_string = $@"URI=file:{files[i]}";
                var con = new SQLiteConnection(con_string);
                con.Open();
                var cmd = new SQLiteCommand(con)
                {
                    CommandText = $"INSERT INTO user(hash, command) VALUES('{hash}', '{data}')"
                };

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public static void ReadRequest(string ip, string port)
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\databases", "*.mdf", SearchOption.AllDirectories);
            int i = 0;
            tryAgain:
            try
            {
                if (i > files.Length)
                {
                    goto end;
                }

                var con_string = $@"URI=file:{files[i]}";
                var con = new SQLiteConnection(con_string);
                con.Open();
                var cmd = new SQLiteCommand(con)
                {
                    CommandText = @"SELECT * FROM user"
                };
                
                var table = cmd.ExecuteReader();
                table.Read();
                Communication.send_response(table[1].ToString(), ip, int.Parse(port));
                con.Close();
                Console.WriteLine("SUCCESS");
                goto end;
            }

            catch
            {
                i++;
                goto tryAgain;         
            }

        end:
            Console.WriteLine("DONE.");
        }

    }
}
