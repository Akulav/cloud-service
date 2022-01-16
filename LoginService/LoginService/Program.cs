using System.Data.SQLite;

namespace LoginService
{
    class Program
    {
        static void Main(string[] args)
        {
            SQLiteConnection connection = Database.connectDB();
            Communications.send_response("whitelist user 13004 localhost","localhost", 130);
            Communications.listen(13004, connection);
        }
    }
}
