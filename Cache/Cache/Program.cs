﻿using System.Data.SQLite;

namespace Cache
{
    class Program
    {
        static void Main(string[] args)
        {
            SQLiteConnection connection = Database.connectDB();
            Communication.send_response("whitelist cache 70 localhost", "localhost", 130);
            Communication.listen(70, connection);
        }
    }
}