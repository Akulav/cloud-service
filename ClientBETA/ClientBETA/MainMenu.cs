using System;
using System.Windows.Forms;

namespace ClientBETA
{
    public partial class MainMenu : Form
    {
        public string identificator;
        public MainMenu(string id)
        {
            this.identificator = id;
            InitializeComponent();
            idlabel.Text = this.identificator;
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        private void upload_Click(object sender, EventArgs e)
        {
            upload.Hide();
            username.Show();
            service.Show();
            password.Show();
            uploadtoserver.Show();
        }

        private void uploadtoserver_Click(object sender, EventArgs e)
        {
            string id = this.identificator;
            Communications.send_data2("localhost", 130, "upload" , idlabel.Text , service.Text ,username.Text, password.Text);
        }

        private void download_Click(object sender, EventArgs e)
        {
            Communications.send_data("localhost", 130, "download", idlabel.Text, null);
            string data = Communications.listen2(13);
            dataBox.Text = data;
        }
    }
}
