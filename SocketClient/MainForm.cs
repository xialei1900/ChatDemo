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

namespace SocketClient
{
    public partial class MainForm : Form
    {
        public Socket ClienetSocket { get; set; }

        public MainForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //客户端链接服务器
            //1 创建socket对象
            ClienetSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            //2 链接服务器
            try
            {
                ClienetSocket.Connect(IPAddress.Parse(textIP.Text), int.Parse(textPort.Text));
            }
            catch (Exception)
            {
                MessageBox.Show("连接失败，请重试...");
                return;
            }

            //3 发送消息，接收消息
            Thread thread = new Thread(new ParameterizedThreadStart(ReceiveData));
            thread.IsBackground = true;
            thread.Start(ClienetSocket);
        }

        #region 接收服务端消息
        public void ReceiveData(object socket)
        {
            var proxSocket = socket as Socket;

            // 建立缓冲区
            byte[] data = new byte[1024 * 1024];

            while (true)
            {
                int len = 0;
                try
                {
                    len = proxSocket.Receive(data, 0, data.Length, SocketFlags.None);
                }
                catch (Exception)
                {

                    //客户端异常退出
                    AppendText($"服务器端[{proxSocket.RemoteEndPoint.ToString()}]异常退出");
                    StopConnect();
                    //结束当前方法，以终结当前接收客户端数据的异步线程
                    return;

                }

                if (len <= 0)
                {
                    //客户端正常退出
                    AppendText($"服务器端[{proxSocket.RemoteEndPoint.ToString()}]正常退出");
                    StopConnect();

                    //结束当前方法，以终结当前接收客户端数据的异步线程
                    return;
                }

                if (data[0] == 1)
                {
                    AppendText($"接收到服务端[{proxSocket.RemoteEndPoint.ToString()}]的消息是：{ProcessReceiveString(data)}");
                }
                else if (data[0] == 2)
                {
                    Shake();
                }
                else if (data[0] == 3)
                {
                    ProcessReceiveFile(data, len);
                }
            }
        }
        #endregion

        #region 处理接收到的字符串
        public string ProcessReceiveString(byte[] data)
        {
            return Encoding.Default.GetString(data, 1, data.Length - 1);
        }
        #endregion

        #region 闪屏
        public void Shake()
        {
            Point oldLocation = this.Location;
            Random r = new Random();

            for (int i = 0; i < 50; i++)
            {
                this.Location = new Point(r.Next(oldLocation
                                                     .X - 10, oldLocation.X + 10), r.Next(oldLocation.Y - 10, oldLocation.Y + 10));
                Thread.Sleep(30);
                this.Location = oldLocation;
            }
        }
        #endregion

        #region 文件处理
        public void ProcessReceiveFile(byte[] data, int len)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.DefaultExt = "txt";
                sfd.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";

                if (sfd.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                byte[] fileData = new byte[len - 1];
                Buffer.BlockCopy(data, 1, fileData, 0, len - 1);
                File.WriteAllBytes(sfd.FileName, fileData);
            }
        }

        #endregion

        #region 发送消息
        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (ClienetSocket.Connected)
            {
                byte[] data = Encoding.Default.GetBytes(textMessage.Text);
                ClienetSocket.Send(data, 0, data.Length, SocketFlags.None);
            }
        } 
        #endregion

        #region 消息输出到界面
        public void AppendText(string text)
        {
            if (textLog.InvokeRequired)
            {
                textLog.BeginInvoke(new Action<string>(s =>
                {
                    this.textLog.Text = $"{s}\r\n{textLog.Text}";
                }), text);
            }
            else
            {
                this.textLog.Text = $"{text}\r\n{textLog.Text}";
            }
        }
        #endregion

        #region 窗体关闭
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopConnect();
        }
        #endregion

        #region 停止连接
        private void StopConnect()
        {
            try
            {
                if (ClienetSocket.Connected)
                {
                    ClienetSocket.Shutdown(SocketShutdown.Both);
                    ClienetSocket.Close(100);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}
