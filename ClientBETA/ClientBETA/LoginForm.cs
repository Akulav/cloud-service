using System;
using System.Windows.Forms;

namespace ClientBETA
{
    public partial class Client : Form
    {
        //Users Service Port 13000
        //Data Port 1300
        //Gateway Port 130
        //Client Port 13
        

        public Client()
        {
            InitializeComponent();
        }

        private void signup_Click(object sender, EventArgs e)
        {   
            Communications.send_data("localhost", 130, "signup" ,username.Text, password.Text);
            string[] data = Communications.listen(13);
            status.Text = StatusCodes.response_codes(data[0]);
        }



        private void login_Click(object sender, EventArgs e)
        {
            Communications.send_data("localhost", 130, "login", username.Text, password.Text);
            string[] data = Communications.listen(13);
            status.Text = StatusCodes.response_codes(data[0]);
            idlabel.Text = data[1];
        }

        private void Client_Load(object sender, EventArgs e)
        {

        }
    }
}