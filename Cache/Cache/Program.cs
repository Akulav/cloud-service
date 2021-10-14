using System;
using System.Data.SqlClient;

namespace Cache
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = Database.connectDB();
            Communication.send_response("whitelist cache 68 localhost", "localhost", 130);
            Communication.listen(68, connection);
        }
    }
}
