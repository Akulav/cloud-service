using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LoginService

{
    class ProtocolStack
    {
        public IPAddress ip;
        public int send_port;

        public String sendData(byte[] buffer)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ep = new IPEndPoint(ip, send_port);
            s.SendTo(buffer, ep);
            s.Close();
            return Encoding.ASCII.GetString(buffer);
        }


    }

}