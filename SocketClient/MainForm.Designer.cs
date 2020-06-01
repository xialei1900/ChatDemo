namespace SocketClient
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.textMessage = new System.Windows.Forms.TextBox();
            this.textLog = new System.Windows.Forms.TextBox();
            this.textPort = new System.Windows.Forms.TextBox();
            this.lbPort = new System.Windows.Forms.Label();
            this.textIP = new System.Windows.Forms.TextBox();
            this.lbIP = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Location = new System.Drawing.Point(609, 397);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(75, 23);
            this.btnSendMessage.TabIndex = 16;
            this.btnSendMessage.Text = "发送";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // textMessage
            // 
            this.textMessage.Location = new System.Drawing.Point(119, 397);
            this.textMessage.Name = "textMessage";
            this.textMessage.Size = new System.Drawing.Size(461, 21);
            this.textMessage.TabIndex = 15;
            // 
            // textLog
            // 
            this.textLog.Location = new System.Drawing.Point(119, 84);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.Size = new System.Drawing.Size(565, 292);
            this.textLog.TabIndex = 14;
            // 
            // textPort
            // 
            this.textPort.Location = new System.Drawing.Point(419, 33);
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(100, 21);
            this.textPort.TabIndex = 13;
            this.textPort.Text = "9090";
            // 
            // lbPort
            // 
            this.lbPort.AutoSize = true;
            this.lbPort.Location = new System.Drawing.Point(355, 36);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(29, 12);
            this.lbPort.TabIndex = 12;
            this.lbPort.Text = "Port";
            // 
            // textIP
            // 
            this.textIP.Location = new System.Drawing.Point(181, 33);
            this.textIP.Name = "textIP";
            this.textIP.Size = new System.Drawing.Size(100, 21);
            this.textIP.TabIndex = 11;
            this.textIP.Text = "127.0.0.1";
            // 
            // lbIP
            // 
            this.lbIP.AutoSize = true;
            this.lbIP.Location = new System.Drawing.Point(117, 36);
            this.lbIP.Name = "lbIP";
            this.lbIP.Size = new System.Drawing.Size(29, 12);
            this.lbIP.TabIndex = 10;
            this.lbIP.Text = "IP：";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(609, 31);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 9;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSendMessage);
            this.Controls.Add(this.textMessage);
            this.Controls.Add(this.textLog);
            this.Controls.Add(this.textPort);
            this.Controls.Add(this.lbPort);
            this.Controls.Add(this.textIP);
            this.Controls.Add(this.lbIP);
            this.Controls.Add(this.btnConnect);
            this.Name = "MainForm";
            this.Text = "客户端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.TextBox textMessage;
        private System.Windows.Forms.TextBox textLog;
        private System.Windows.Forms.TextBox textPort;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.TextBox textIP;
        private System.Windows.Forms.Label lbIP;
        private System.Windows.Forms.Button btnConnect;
    }
}