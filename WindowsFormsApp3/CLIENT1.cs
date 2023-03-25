using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp3
{

    public partial class CLIENT1 : Form
    {
        private UdpClient client;
        private Thread listeningThread;
        private List<string> messages = new List<string>();


        public CLIENT1()
        {
           
            InitializeComponent();
            client = new UdpClient(0);
            listeningThread = new Thread(new ThreadStart(ListenForMessages));
            listeningThread.Start();
            listView1.View = View.Details;
            listView1.Columns.Add("Messenger", -2, HorizontalAlignment.Left);

        }
        


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            client.Close();
            listeningThread.Abort();
        }
        private void SendMessage()
        {
            string message = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (Control.ModifierKeys == Keys.Shift && message.EndsWith("\r"))
            {
                message = message.Substring(0, message.Length - 1) + "\n";
            }

            message = "Client: " + message;
            byte[] data = Encoding.ASCII.GetBytes(message);
            client.Send(data, data.Length, "localhost", 4747);

            Invoke(new Action(() =>
            {
                if (!messages.Contains(message))
                {
                    messages.Add(message);
                }
                DisplayMessages();
            }));
            textBox1.Text = "";
        }
        private void DisplayMessages()
        {
            listView1.Items.Clear();
            foreach (string message in messages)
            {
                listView1.Items.Add(new ListViewItem(new[] { message }));
            }
        }
        private void ListenForMessages()
        {
            while (true)
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref remoteEndPoint);
                string message = Encoding.ASCII.GetString(data);

                Invoke(new Action(() =>
                {
                    if (!messages.Contains(message))
                    {
                        messages.Add(message);
                    }
                    DisplayMessages();
                }));
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                SendMessage();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bt1.Enabled = !string.IsNullOrEmpty(textBox1.Text);
        }

        private void bt1_Click(object sender, EventArgs e)
        {
            SendMessage();
        }
        
        private void CLIENT1_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            button2.Enabled = false;
            bt1.Enabled = false;
            button4.Enabled = false;
            listView1.Enabled = false;
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Enabled)
            {
               
                textBox1.Enabled = true;
                bt1.Enabled = true;
                button4.Enabled = true;
                listView1.Enabled=true;
                MessageBox.Show("Connection Successful: " + "Success", "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                
                textBox1.Enabled = false;
                bt1.Enabled = false;
                button4.Enabled=false;
                listView1 .Enabled = false;
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to exit this program?: " + "Exit", "UDP Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Close(); 
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox3.Text);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox3.Text);

        }
    }
}
