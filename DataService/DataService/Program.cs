using Community.CsharpSqlite.SQLiteClient;

namespace DataService
{
    class Program
    {
        static void Main(string[] args)
        {
            SqliteConnection connection = Database.connectDB();
            Communications.send_response("whitelist data 1303 localhost", "localhost", 130);
            Communications.listen(1303, connection);
        }
        
    }
}
