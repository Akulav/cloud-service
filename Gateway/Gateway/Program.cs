using System.IO;

namespace Gateway
{
    class Program
    {
        static void Main(string[] args)
        {
            File.WriteAllBytes("caches.mdf", Gateway.Properties.Resources.caches);
            File.WriteAllBytes("datas.mdf", Gateway.Properties.Resources.datas);
            File.WriteAllBytes("logins.mdf", Gateway.Properties.Resources.logins);
            Communication.listen(130);
        }
    }
}
