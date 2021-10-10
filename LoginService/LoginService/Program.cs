using System;
using System.Data.SqlClient;
namespace LoginService
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = Database.connectDB();
            Communications.listen(13000, connection);
        }
    }
}
