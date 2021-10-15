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

        public string id;

        public Client()
        {
            InitializeComponent();
        }

        private void signup_Click(object sender, EventArgs e)
        {
            Communications.send_data("localhost", 130, "signup", username.Text, password.Text);
            string[] data = Communications.listen(13);
            status.Text = StatusCodes.response_codes(data[0]);
        }



        private void login_Click(object sender, EventArgs e)
        {
            Communications.send_data("localhost", 130, "login", username.Text, password.Text);
            string[] data = Communications.listen(13);
            status.Text = StatusCodes.response_codes(data[0]);

            if (data.Length > 1)
            {
                Communications.send_data("localhost", 130, "connect", data[1], null);
                idlabel.Text = data[1];
                this.id = data[1];
                MainMenu mm = new MainMenu(id);
                mm.Show();
                //this.Hide();
            }
            
        }

        private void Client_Load(object sender, EventArgs e)
        {
            //Communications.send_data("localhost", 130, "health", null, null);
        }

        
    }
}