using System.Data.SqlClient;

namespace Gateway
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = Database.connectDB();
            Communication.listen(130, connection);
        }
    }
}
