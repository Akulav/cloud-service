using System.Data.SqlClient;

namespace Cache
{
    class Program
    {
        static void Main(string[] args)
        {
            Communication.listen(112);
        }
    }
}
