using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientBETA
{
    public partial class Client : Form
    {


        public Client()
        {
            InitializeComponent();
        }


        public void listen()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 13);
            tcpListener.Start();

                        Socket client = tcpListener.AcceptSocket();
                        Console.WriteLine("Connection accepted.");
                        char[] user_data = new char[999];

                     
                            byte[] data = new byte[100];
                            int size = client.Receive(data);
                            Console.WriteLine("Recieved data!");

                            for (int i = 0; i < size; i++)
                            {
                                user_data[i] = Convert.ToChar(data[i]);
                            }

                            string str = new string(user_data);





                            status.Text = str;
            tcpListener.Stop();
            client.Close();


                    
      
                        
          

        }

        private void signup_Click(object sender, EventArgs e)
        {
            
            TcpClient tcpClient = new TcpClient("localHost", 130);
            using (NetworkStream ns = tcpClient.GetStream())
            {

                using (
                    BufferedStream bs = new BufferedStream(ns))
                {
                    byte[] messageBytesToSend = Encoding.UTF8.GetBytes("signup" + " " + username.Text + " " + password.Text);
                    bs.Write(messageBytesToSend, 0, messageBytesToSend.Length);

                }

            }

            listen();
        }



        private void login_Click(object sender, EventArgs e)
        {
            TcpClient tcpClient = new TcpClient("localHost", 130);
            using (NetworkStream ns = tcpClient.GetStream())
            {

                using (
                    BufferedStream bs = new BufferedStream(ns))
                {
                    byte[] messageBytesToSend = Encoding.UTF8.GetBytes("login" + " " + username.Text + " " + password.Text);
                    bs.Write(messageBytesToSend, 0, messageBytesToSend.Length);

                }

            }

            listen();

            
        }
    }
}
