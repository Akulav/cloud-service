
namespace ClientBETA
{
    partial class Client
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
            this.signup = new System.Windows.Forms.Button();
            this.login = new System.Windows.Forms.Button();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.status = new System.Windows.Forms.Label();
            this.idlabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // signup
            // 
            this.signup.Location = new System.Drawing.Point(279, 326);
            this.signup.Name = "signup";
            this.signup.Size = new System.Drawing.Size(75, 23);
            this.signup.TabIndex = 0;
            this.signup.Text = "signup";
            this.signup.UseVisualStyleBackColor = true;
            this.signup.Click += new System.EventHandler(this.signup_Click);
            // 
            // login
            // 
            this.login.Location = new System.Drawing.Point(279, 355);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(75, 23);
            this.login.TabIndex = 1;
            this.login.Text = "login";
            this.login.UseVisualStyleBackColor = true;
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(223, 274);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(192, 20);
            this.username.TabIndex = 2;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(223, 300);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(192, 20);
            this.password.TabIndex = 3;
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Location = new System.Drawing.Point(291, 381);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(50, 13);
            this.status.TabIndex = 4;
            this.status.Text = "STATUS";
            // 
            // idlabel
            // 
            this.idlabel.AutoSize = true;
            this.idlabel.Location = new System.Drawing.Point(12, 668);
            this.idlabel.Name = "idlabel";
            this.idlabel.Size = new System.Drawing.Size(18, 13);
            this.idlabel.TabIndex = 5;
            this.idlabel.Text = "ID";
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 690);
            this.Controls.Add(this.idlabel);
            this.Controls.Add(this.status);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.login);
            this.Controls.Add(this.signup);
            this.Name = "Client";
            this.Text = "Client";
            this.Load += new System.EventHandler(this.Client_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button signup;
        private System.Windows.Forms.Button login;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Label idlabel;
    }
}

