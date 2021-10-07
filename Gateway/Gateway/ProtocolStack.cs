using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Gateway

{
    class ProtocolStack
    {
        public IPAddress ip;
        public int receive_port;
        public int send_port;


        /*
        public string receiveData()
        {
            UdpClient listener = new UdpClient(receive_port);
            IPEndPoint groupEP = new IPEndPoint(ip, receive_port);
            byte[] bytes = listener.Receive(ref groupEP);

            listener.Close();
            string data = Encoding.ASCII.GetString(bytes, 0, bytes.Length);

            listener.Close();
            return data;
        }
        */

    }

}