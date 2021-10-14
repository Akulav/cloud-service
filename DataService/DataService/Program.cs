using System.Data.SqlClient;

namespace DataService
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = Database.connectDB();
            Communications.send_response("whitelist data 1301 localhost", "localhost", 130);
            Communications.listen(1301, connection);
        }
        
    }
}
