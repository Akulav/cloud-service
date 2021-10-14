using System.Data.SqlClient;

namespace Gateway
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = Database.connectDB();
            string[] user = Communication.getUser(connection);
            string[] data = Communication.getData(connection);
            string[] cache = Communication.getCache(connection);
            Communication.listen(130, connection, user, data, cache);
        }
    }
}
