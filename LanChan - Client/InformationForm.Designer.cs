namespace LanChat___Client
{
    partial class InformationForm
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
            this.IPInput = new System.Windows.Forms.TextBox();
            this.IPText = new System.Windows.Forms.Label();
            this.NameText = new System.Windows.Forms.Label();
            this.NameInput = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // IPInput
            // 
            this.IPInput.Location = new System.Drawing.Point(193, 119);
            this.IPInput.Name = "IPInput";
            this.IPInput.Size = new System.Drawing.Size(400, 22);
            this.IPInput.TabIndex = 0;
            this.IPInput.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // IPText
            // 
            this.IPText.AutoSize = true;
            this.IPText.Font = new System.Drawing.Font("Leelawadee UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPText.Location = new System.Drawing.Point(289, 76);
            this.IPText.Name = "IPText";
            this.IPText.Size = new System.Drawing.Size(218, 28);
            this.IPText.TabIndex = 1;
            this.IPText.Text = "Nhap dia chi IP Server";
            this.IPText.Click += new System.EventHandler(this.label1_Click);
            // 
            // NameText
            // 
            this.NameText.AutoSize = true;
            this.NameText.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.NameText.Font = new System.Drawing.Font("Leelawadee UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameText.Location = new System.Drawing.Point(339, 192);
            this.NameText.Name = "NameText";
            this.NameText.Size = new System.Drawing.Size(99, 28);
            this.NameText.TabIndex = 3;
            this.NameText.Text = "Nhap ten";
            // 
            // NameInput
            // 
            this.NameInput.Location = new System.Drawing.Point(193, 236);
            this.NameInput.Name = "NameInput";
            this.NameInput.Size = new System.Drawing.Size(400, 22);
            this.NameInput.TabIndex = 2;
            // 
            // ConnectButton
            // 
            this.ConnectButton.Font = new System.Drawing.Font("Leelawadee UI", 12F, System.Drawing.FontStyle.Bold);
            this.ConnectButton.Location = new System.Drawing.Point(313, 304);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(150, 48);
            this.ConnectButton.TabIndex = 4;
            this.ConnectButton.Text = "Ket noi";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // InformationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.NameText);
            this.Controls.Add(this.NameInput);
            this.Controls.Add(this.IPText);
            this.Controls.Add(this.IPInput);
            this.Name = "InformationForm";
            this.Text = "InfomationForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox IPInput;
        private System.Windows.Forms.Label IPText;
        private System.Windows.Forms.Label NameText;
        private System.Windows.Forms.TextBox NameInput;
        private System.Windows.Forms.Button ConnectButton;
    }
}