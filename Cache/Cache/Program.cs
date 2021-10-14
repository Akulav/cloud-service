using System;
using System.Data.SqlClient;

namespace Cache
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = Database.connectDB();
            Communication.listen(69, connection);
        }
    }
}
