using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Gateway
{
    class Communication
    {
        public static void send_to_user(string data)
        {

            TcpClient tcpClient = new TcpClient("localHost", 13000);
            using (NetworkStream ns = tcpClient.GetStream())
            {

                using (
                    BufferedStream bs = new BufferedStream(ns))
                {
                    byte[] messageBytesToSend = Encoding.UTF8.GetBytes(data);
                    bs.Write(messageBytesToSend, 0, messageBytesToSend.Length);
                }

            }

        }


        public static void send_to_client(string data)
        {

            TcpClient tcpClient = new TcpClient("localHost", 13);
            using (NetworkStream ns = tcpClient.GetStream())
            {

                using (
                    BufferedStream bs = new BufferedStream(ns))
                {
                    byte[] messageBytesToSend = Encoding.UTF8.GetBytes(data);
                    bs.Write(messageBytesToSend, 0, messageBytesToSend.Length);
                }

            }
            tcpClient.Close();

        }
        /*
        public static string receive_from_client()
        {
            ServerClientSync sc = new ServerClientSync();
            sc.ip = IPAddress.Parse("192.168.10.107");
            sc.send_port = 11001;
            sc.receive_port = 11000;
            string str = sc.syncWithServer();
            return str;
        }
        */

        public static void router(string[] data, string data_string)
        {
            if (data[0]=="signup" || data[0] == "login")
            {
                send_to_user(data_string);          
            }

            else
            {
                send_to_client(data_string);
            }
        }

    }
}
