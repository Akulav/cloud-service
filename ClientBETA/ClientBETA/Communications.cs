using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientBETA
{
    class Communications
    {
        public static void send_data(string ip, int port, string command, string arg1, string arg2)
        {
            TcpClient tcpClient = new TcpClient(ip, port);
            string data = command + " " + arg1 + " " + arg2 + " ";
            string data_hash = data + Crypto.GetHashString(data);

            using (NetworkStream ns = tcpClient.GetStream())
            {

                using (
                    BufferedStream bs = new BufferedStream(ns))
                {
                    byte[] messageBytesToSend = Encoding.UTF8.GetBytes(data_hash);
                    bs.Write(messageBytesToSend, 0, messageBytesToSend.Length);
                }

            }
        }

        public static void send_data2(string ip, int port, string command, string arg1, string arg2, string arg3, string arg4)
        {
            TcpClient tcpClient = new TcpClient(ip, port);
            using (NetworkStream ns = tcpClient.GetStream())
            {

                using (
                    BufferedStream bs = new BufferedStream(ns))
                {
                    byte[] messageBytesToSend = Encoding.UTF8.GetBytes(command + " " + arg1 + " " + arg2 +" "+ arg3 + " " + arg4);
                    bs.Write(messageBytesToSend, 0, messageBytesToSend.Length);
                }

            }
            
        }

        public static string[] listen(int port)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            Socket client = tcpListener.AcceptSocket();
            char[] user_data = new char[999];

            byte[] data = new byte[100];
            int size = client.Receive(data);
            Console.WriteLine("Received data!");

            for (int i = 0; i < size; i++)
            {
                user_data[i] = Convert.ToChar(data[i]);
            }

            string str = new string(user_data);
            tcpListener.Stop();
            client.Close();

            string[] statusid = DataProcessing.GetID(str);

            return statusid;
        }

        public static string listen2(int port)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            Socket client = tcpListener.AcceptSocket();
            char[] user_data = new char[999];

            byte[] data = new byte[100];
            int size = client.Receive(data);
            Console.WriteLine("Received data!");

            for (int i = 0; i < size; i++)
            {
                user_data[i] = Convert.ToChar(data[i]);
            }

            string str = new string(user_data);
            tcpListener.Stop();
            client.Close();

            

            return str;
        }
    }
}
