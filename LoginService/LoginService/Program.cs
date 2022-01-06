using System.Data.SQLite;

namespace LoginService
{
    class Program
    {
        static void Main(string[] args)
        {
            SQLiteConnection connection = Database.connectDB();
            Communications.send_response("whitelist user 13003 localhost","localhost", 130);
            Communications.listen(13003, connection);
        }
    }
}
