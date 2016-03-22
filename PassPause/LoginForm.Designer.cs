namespace Betarium.PassPause
{
    partial class LoginForm
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
            this.UserIdLabel = new System.Windows.Forms.Label();
            this.UserIdField = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PasswordField = new System.Windows.Forms.TextBox();
            this.EnterButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // UserIdLabel
            // 
            this.UserIdLabel.AutoSize = true;
            this.UserIdLabel.Location = new System.Drawing.Point(12, 18);
            this.UserIdLabel.Name = "UserIdLabel";
            this.UserIdLabel.Size = new System.Drawing.Size(73, 12);
            this.UserIdLabel.TabIndex = 0;
            this.UserIdLabel.Text = "ユーザー名(&U)";
            // 
            // UserIdField
            // 
            this.UserIdField.Location = new System.Drawing.Point(104, 15);
            this.UserIdField.Name = "UserIdField";
            this.UserIdField.Size = new System.Drawing.Size(140, 19);
            this.UserIdField.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "パスワード(&P)";
            // 
            // PasswordField
            // 
            this.PasswordField.Location = new System.Drawing.Point(104, 50);
            this.PasswordField.Name = "PasswordField";
            this.PasswordField.Size = new System.Drawing.Size(140, 19);
            this.PasswordField.TabIndex = 3;
            this.PasswordField.UseSystemPasswordChar = true;
            // 
            // EnterButton
            // 
            this.EnterButton.Location = new System.Drawing.Point(88, 93);
            this.EnterButton.Name = "EnterButton";
            this.EnterButton.Size = new System.Drawing.Size(75, 23);
            this.EnterButton.TabIndex = 4;
            this.EnterButton.Text = "OK";
            this.EnterButton.UseVisualStyleBackColor = true;
            this.EnterButton.Click += new System.EventHandler(this.EnterButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(169, 93);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 5;
            this.CancelButton.Text = "キャンセル";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // LoginForm
            // 
            this.AcceptButton = this.EnterButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 128);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.EnterButton);
            this.Controls.Add(this.PasswordField);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.UserIdField);
            this.Controls.Add(this.UserIdLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.Text = "ログイン";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UserIdLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PasswordField;
        private System.Windows.Forms.Button EnterButton;
        private System.Windows.Forms.Button CancelButton;
        public System.Windows.Forms.TextBox UserIdField;
    }
}