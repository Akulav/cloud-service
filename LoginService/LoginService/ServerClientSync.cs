using System;
using System.Net;
using System.Text;
using System.Threading;

namespace LoginService
{
    class ServerClientSync
    {
        public int connected = 0;
        public IPAddress ip;
        public int receive_port;
        public int send_port;


        public void syncWithClient(string data)
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;
           
                
            byte[] sendbuf = Encoding.ASCII.GetBytes(data);
            sendDataRetrans(sendbuf);
           
        }



        public void sendDataRetrans(byte[] information)
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;

            string[] response;

            ps.sendData(information);
            

        }

    }
}