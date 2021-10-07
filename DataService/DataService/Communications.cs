using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace DataService
{
    class Communications
    {

        public static void send_data(string ip, int port, string command, string arg1)
        {
            TcpClient tcpClient = new TcpClient(ip, port);
            using (NetworkStream ns = tcpClient.GetStream())
            {

                using (
                    BufferedStream bs = new BufferedStream(ns))
                {
                    byte[] messageBytesToSend = Encoding.UTF8.GetBytes(command + " " + arg1);
                    bs.Write(messageBytesToSend, 0, messageBytesToSend.Length);
                }

            }
        }
    }
}
