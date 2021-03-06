﻿namespace Betarium.PassPause
{
    partial class PasswordForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.PasswordField = new System.Windows.Forms.TextBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.PasswordConfirmField = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "パスワード";
            // 
            // PasswordField
            // 
            this.PasswordField.Location = new System.Drawing.Point(37, 59);
            this.PasswordField.Name = "PasswordField";
            this.PasswordField.Size = new System.Drawing.Size(278, 19);
            this.PasswordField.TabIndex = 0;
            this.PasswordField.UseSystemPasswordChar = true;
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(240, 146);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 2;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // PasswordConfirmField
            // 
            this.PasswordConfirmField.Location = new System.Drawing.Point(37, 109);
            this.PasswordConfirmField.Name = "PasswordConfirmField";
            this.PasswordConfirmField.Size = new System.Drawing.Size(278, 19);
            this.PasswordConfirmField.TabIndex = 1;
            this.PasswordConfirmField.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "パスワード（確認）";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(268, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "暗号化に使用するマスターパスワードを入力してください。";
            // 
            // PasswordForm
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 181);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.PasswordConfirmField);
            this.Controls.Add(this.PasswordField);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordForm";
            this.Text = "パスワード";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PasswordField;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.TextBox PasswordConfirmField;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}