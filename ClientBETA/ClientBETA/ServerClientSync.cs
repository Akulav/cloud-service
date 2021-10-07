using System;
using System.Net;

namespace ClientBETA
{
    class ServerClientSync
    {
        public int connected = 0;
        public IPAddress ip;
        public int receive_port;
        public int send_port;

        public string syncWithServer()
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;
            return receiveDataRetrans();
        }


        public String receiveDataRetrans()
        {
            ProtocolStack ps = new ProtocolStack();
            ps.ip = ip;
            ps.receive_port = receive_port;
            ps.send_port = send_port;

            string response;


            response = ps.receiveData();
            return response;
        }
    }
}