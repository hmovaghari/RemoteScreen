using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace RemoteScreen
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem btnStop;
        private ToolStripMenuItem btnExit;
        private Timer tickCapture;
        private Timer tickReader;

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
            this.components = new System.ComponentModel.Container();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnStop = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tickCapture = new System.Windows.Forms.Timer(this.components);
            this.tickReader = new System.Windows.Forms.Timer(this.components);
            this.tickCaptureMouse = new System.Windows.Forms.Timer(this.components);
            this.btnSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = global::RemoteScreen.Properties.Resources.RemoteScreen;
            this.notifyIcon1.Text = "Remote Screen";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStop,
            this.btnSettings,
            this.btnExit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 92);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // btnStop
            // 
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(180, 22);
            this.btnStop.Text = "btnStop";
            // 
            // btnExit
            // 
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(180, 22);
            this.btnExit.Text = "Exit";
            // 
            // btnSettings
            // 
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(180, 22);
            this.btnSettings.Text = "Settings";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 154);
            this.Icon = global::RemoteScreen.Properties.Resources.RemoteScreen;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "Form1";
            this.Text = "Remote Screen";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_Closed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.Disposed += new System.EventHandler(this.Form1_Disposed);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Timer tickCaptureMouse;
        private ToolStripMenuItem btnSettings;
    }
}