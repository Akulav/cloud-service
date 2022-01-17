using Community.CsharpSqlite.SQLiteClient;

namespace DB1
{
    class Program
    {
        static void Main(string[] args)
        {
            SqliteConnection connection = Database.connectDB();
            Communications.listen(15002, connection);
        }
    }
}
