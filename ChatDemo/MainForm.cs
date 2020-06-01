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

namespace ChatDemo
{
    public partial class MainForm : Form
    {
        List<Socket> _clientProxSocketList = new List<Socket>();
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //1 创建socket
            Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            //2 绑定端口
            socket.Bind(new IPEndPoint(IPAddress.Parse(textIP.Text),int.Parse(textPort.Text)));

            //3 开启监听
            socket.Listen(10);//连接等待队列最大长度

            //4 开始接受客户端连接
            ThreadPool.QueueUserWorkItem(AcceptClientConnection, socket);

        }

        #region 接收客户端连接
        public void AcceptClientConnection(object socket)
        {
            var serverSocket = socket as Socket;

            this.AppendText("服务器端开始接收客户端链接。");

            while (true)
            {
                var proxSocket = serverSocket.Accept();
                this.AppendText($"客户端[{proxSocket.RemoteEndPoint.ToString()}]链接上了");
                _clientProxSocketList.Add(proxSocket);

                //不停的接收当前链接的客户端发来的消息
                ThreadPool.QueueUserWorkItem(ReceiveData, proxSocket);
            }
        }
        #endregion

        #region 接收客户端消息
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
                    AppendText($"客户端[{proxSocket.RemoteEndPoint.ToString()}]异常退出");
                    _clientProxSocketList.Remove(proxSocket);
                    StopConnect(proxSocket);

                    //结束当前方法，以终结当前接收客户端数据的异步线程
                    return;

                }

                if (len <= 0)
                {
                    //客户端正常退出
                    AppendText($"客户端[{proxSocket.RemoteEndPoint.ToString()}]正常退出");
                    _clientProxSocketList.Remove(proxSocket);
                    StopConnect(proxSocket);

                    //结束当前方法，以终结当前接收客户端数据的异步线程
                    return;
                }

                string str = Encoding.Default.GetString(data, 0, len);
                AppendText($"接收到客户端[{proxSocket.RemoteEndPoint.ToString()}]的消息是：{str}");
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

        #region 发送消息
        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            foreach (var proxSocket in _clientProxSocketList)
            {
                if (proxSocket.Connected)
                {
                    //原始字符串转换成字节数组
                    byte[] data = Encoding.Default.GetBytes(textMessage.Text);
                    //加上协议头部
                    byte[] result = new byte[data.Length + 1];
                    //设置当前协议头部：1 代表字符串
                    result[0] = 1;
                    //把原始的数据放到最终的字节数组里
                    Buffer.BlockCopy(data, 0, result, 1, data.Length);

                    proxSocket.Send(result, 0, result.Length, SocketFlags.None);
                }
            }
        }
        #endregion

        #region 发送窗体抖动
        private void btnSendShake_Click(object sender, EventArgs e)
        {
            foreach (var proxSocket in _clientProxSocketList)
            {
                if (proxSocket.Connected)
                {
                    proxSocket.Send(new byte[] { 2 }, SocketFlags.None);
                }
            }
        }
        #endregion

        #region 发送文件
        private void btnSendFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                byte[] data = File.ReadAllBytes(ofd.FileName);
                byte[] result = new byte[data.Length + 1];
                //设置当前协议头部：3 代表文件
                result[0] = 3;
                //把原始的数据放到最终的字节数组里
                Buffer.BlockCopy(data, 0, result, 1, data.Length);

                foreach (var proxSocket in _clientProxSocketList)
                {
                    if (!proxSocket.Connected)
                    {
                        continue;
                    }

                    proxSocket.Send(result, SocketFlags.None);
                }
            }
        }
        #endregion

        #region 停止连接
        private void StopConnect(Socket proxSocket)
        {
            try
            {
                if (proxSocket.Connected)
                {
                    proxSocket.Shutdown(SocketShutdown.Both);
                    proxSocket.Close(100);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}
