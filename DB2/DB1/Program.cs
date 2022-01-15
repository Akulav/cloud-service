using System.Data.SQLite;

namespace DB1
{
    class Program
    {
        static void Main(string[] args)
        {
            SQLiteConnection connection = Database.connectDB();
            Communications.send_response("whitelist db 15000 localhost","localhost", 130);
            Communications.listen(15000, connection);
        }
    }
}
