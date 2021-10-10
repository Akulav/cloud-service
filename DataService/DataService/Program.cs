using System.Data.SqlClient;

namespace DataService
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = Database.connectDB();
            Communications.listen(1300, connection);
        }
        
    }
}
