using System.Data.SqlClient;
namespace LoginService
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = Database.connectDB();
            Communications.send_response("whitelist user 1001 localhost","localhost", 130);
            Communications.listen(13001, connection);
        }
    }
}
