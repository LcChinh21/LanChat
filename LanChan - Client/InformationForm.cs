using LanChat.Networking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanChat___Client
{
    public partial class InformationForm : Form
    {
        public InformationForm()
        {
            InitializeComponent();
            ConnectButton.Click += ConnectButton_Click;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            ClientSocket client = new ClientSocket();
            while (!client.IsConnected)
            {
                client.Connect(IPInput.Text, 5000, NameInput.Text);
            }
            Form1 form1 = new Form1();
            form1.Show();
        }
    }
}
