using System.Data.SQLite;

namespace DB1
{
    class Program
    {
        static void Main(string[] args)
        {
            SQLiteConnection connection = Database.connectDB();
            Communications.listen(15002, connection);
        }
    }
}
