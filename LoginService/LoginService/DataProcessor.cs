using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LoginService
{
    class DataProcessor
    {

        public static void send_response(string data)
        {
            ServerClientSync sc = new ServerClientSync();
            sc.ip = IPAddress.Parse("192.168.10.107");
            sc.send_port = 11000;
            sc.receive_port = 11001;
            sc.syncWithClient(data);
        }
    }
}
