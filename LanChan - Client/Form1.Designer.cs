using System.Windows.Forms;

namespace LanChat___Client
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.StatusPanel = new System.Windows.Forms.Panel();
            this.OnlineUsersBox = new System.Windows.Forms.ListBox();
            this.StatusHeader = new System.Windows.Forms.Label();
            this.ChatPanel = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.ChatHeader = new System.Windows.Forms.Label();
            this.StatusPanel.SuspendLayout();
            this.ChatPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusPanel
            // 
            this.StatusPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StatusPanel.Controls.Add(this.OnlineUsersBox);
            this.StatusPanel.Controls.Add(this.StatusHeader);
            this.StatusPanel.Cursor = System.Windows.Forms.Cursors.IBeam;
            resources.ApplyResources(this.StatusPanel, "StatusPanel");
            this.StatusPanel.ForeColor = System.Drawing.Color.Coral;
            this.StatusPanel.Name = "StatusPanel";
            // 
            // OnlineUsersBox
            // 
            this.OnlineUsersBox.FormattingEnabled = true;
            resources.ApplyResources(this.OnlineUsersBox, "OnlineUsersBox");
            this.OnlineUsersBox.Name = "OnlineUsersBox";
            this.OnlineUsersBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // StatusHeader
            // 
            resources.ApplyResources(this.StatusHeader, "StatusHeader");
            this.StatusHeader.ForeColor = System.Drawing.Color.SeaGreen;
            this.StatusHeader.Name = "StatusHeader";
            // 
            // ChatPanel
            // 
            this.ChatPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ChatPanel.Controls.Add(this.textBox1);
            this.ChatPanel.Controls.Add(this.listBox1);
            this.ChatPanel.Controls.Add(this.ChatHeader);
            resources.ApplyResources(this.ChatPanel, "ChatPanel");
            this.ChatPanel.Name = "ChatPanel";
            this.ChatPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // listBox1
            // 
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Name = "listBox1";
            // 
            // ChatHeader
            // 
            resources.ApplyResources(this.ChatHeader, "ChatHeader");
            this.ChatHeader.Name = "ChatHeader";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ChatPanel);
            this.Controls.Add(this.StatusPanel);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.StatusPanel.ResumeLayout(false);
            this.StatusPanel.PerformLayout();
            this.ChatPanel.ResumeLayout(false);
            this.ChatPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel StatusPanel;
        private System.Windows.Forms.Panel ChatPanel;
        private System.Windows.Forms.Label StatusHeader;
        private System.Windows.Forms.ListBox OnlineUsersBox;
        private System.Windows.Forms.Label ChatHeader;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox listBox1;
    }
}

