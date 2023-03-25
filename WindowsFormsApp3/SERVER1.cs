using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp3
{
    public partial class SERVER1 : Form
    {
        private UdpClient server;
        private Thread listeningThread;
        private List<string> messages = new List<string>();
        private IPEndPoint clientEndPoint;
        public SERVER1()
        {
            InitializeComponent();
            server = new UdpClient(4747);
            listeningThread = new Thread(new ThreadStart(ListenForMessages));
            listeningThread.Start();

            listView1.View = View.Details;
            listView1.Columns.Add("Messenger", -2, HorizontalAlignment.Left);

            
        }


        private void SERVER1_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Close();
            listeningThread.Abort();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                SendMessage();
            }
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

            byte[] data = Encoding.ASCII.GetBytes(message);
            message = "Server: " + message;
            data = Encoding.ASCII.GetBytes(message);
            server.Send(data, data.Length, "localhost", 4747);
            if (clientEndPoint != null)
            {
                server.Send(data, data.Length, clientEndPoint);
            }
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
                byte[] data = server.Receive(ref remoteEndPoint);
                string message = Encoding.ASCII.GetString(data);

                if (clientEndPoint == null)
                {
                    clientEndPoint = remoteEndPoint;
                }

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            button1.Enabled = !string.IsNullOrEmpty(textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void SERVER1_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            //button3.Enabled = false;
            button1.Enabled = false;
            button5.Enabled = false;
            listView1.Enabled = false;
          
        }
        
        
        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Enabled)
            {
                
                textBox1.Enabled = true;
                button1.Enabled = true;
                button5.Enabled = true;
                listView1.Enabled = true;


                MessageBox.Show("Connection Successful: " + "Success", "UDP Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                
            }
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to exit this program?: " + "Exit", "UDP Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Close();
            }

        }

        
    }
     
    }

