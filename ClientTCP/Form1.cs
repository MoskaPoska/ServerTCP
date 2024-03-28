using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;

namespace ClientTCP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Process.Start("ServerTCP.exe");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect("192.168.56.1", 11000);
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(Encoding.UTF8.GetBytes(textBox1.Text));
                textBox1.Clear();
            }
            catch (SocketException ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                tcpClient.Close();
            }
        }
    }
}