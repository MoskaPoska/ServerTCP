using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerTCP
{
    public partial class Form1 : Form
    {
        Thread thread;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (thread != null) return;
            thread = new Thread(ServerFunc);
            thread.IsBackground = true;
            thread.Start();
            Text = "Server is start";
        }

        private void ServerFunc()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Parse("192.168.56.1"), 11000);
            try
            {
                tcpListener.Start(5);
                do
                {
                    if(tcpListener.Pending())
                    {
                        TcpClient tcpClient = tcpListener.AcceptTcpClient();
                        byte[] buff = new byte[1024];
                        NetworkStream ns = tcpClient.GetStream();
                        int len = ns.Read(buff, 0, buff.Length);
                        StringBuilder sb = new StringBuilder();
                        string receivedMessage = Encoding.UTF8.GetString(buff, 0, len);
                        sb.AppendLine($"{len} byte received fron {tcpClient.Client.RemoteEndPoint}");
                        sb.AppendLine(Encoding.UTF8.GetString(buff, 0, len));
                        textBox1.BeginInvoke(new Action<string>(AddText),sb.ToString());
                        if (receivedMessage.Trim().ToUpper() == "EXIT") 
                        {
                            tcpClient.Client.Shutdown(SocketShutdown.Receive);
                            tcpClient.Close();
                            Application.Exit();
                            return;
                        }
                       
                    }
                } while (true);
            }
            catch (SystemException ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                tcpListener.Stop();
            }
        }

        private void AddText(string obj)
        {
            StringBuilder sb = new StringBuilder(textBox1.Text);
            sb.AppendLine(obj);
            textBox1.Text = sb.ToString();
        }
    }
}