
namespace ClientBETA
{
    partial class MainMenu
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
            this.idlabel = new System.Windows.Forms.Label();
            this.download = new System.Windows.Forms.Button();
            this.service = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.uploadtoserver = new System.Windows.Forms.Button();
            this.dataBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // idlabel
            // 
            this.idlabel.AutoSize = true;
            this.idlabel.Location = new System.Drawing.Point(12, 477);
            this.idlabel.Name = "idlabel";
            this.idlabel.Size = new System.Drawing.Size(18, 13);
            this.idlabel.TabIndex = 5;
            this.idlabel.Text = "ID";
            // 
            // download
            // 
            this.download.Location = new System.Drawing.Point(157, 205);
            this.download.Name = "download";
            this.download.Size = new System.Drawing.Size(233, 51);
            this.download.TabIndex = 7;
            this.download.Text = "View Data";
            this.download.UseVisualStyleBackColor = true;
            this.download.Click += new System.EventHandler(this.download_Click);
            // 
            // service
            // 
            this.service.Location = new System.Drawing.Point(186, 70);
            this.service.Name = "service";
            this.service.Size = new System.Drawing.Size(170, 20);
            this.service.TabIndex = 8;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(186, 122);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(170, 20);
            this.password.TabIndex = 9;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(186, 96);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(170, 20);
            this.username.TabIndex = 10;
            // 
            // uploadtoserver
            // 
            this.uploadtoserver.Location = new System.Drawing.Point(157, 148);
            this.uploadtoserver.Name = "uploadtoserver";
            this.uploadtoserver.Size = new System.Drawing.Size(233, 51);
            this.uploadtoserver.TabIndex = 11;
            this.uploadtoserver.Text = "Upload";
            this.uploadtoserver.UseVisualStyleBackColor = true;
            this.uploadtoserver.Click += new System.EventHandler(this.uploadtoserver_Click);
            // 
            // dataBox
            // 
            this.dataBox.Location = new System.Drawing.Point(157, 262);
            this.dataBox.Multiline = true;
            this.dataBox.Name = "dataBox";
            this.dataBox.Size = new System.Drawing.Size(233, 109);
            this.dataBox.TabIndex = 12;
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 501);
            this.Controls.Add(this.dataBox);
            this.Controls.Add(this.uploadtoserver);
            this.Controls.Add(this.username);
            this.Controls.Add(this.password);
            this.Controls.Add(this.service);
            this.Controls.Add(this.download);
            this.Controls.Add(this.idlabel);
            this.Name = "MainMenu";
            this.Text = "V";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label idlabel;
        private System.Windows.Forms.Button download;
        private System.Windows.Forms.TextBox service;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Button uploadtoserver;
        private System.Windows.Forms.TextBox dataBox;
    }
}