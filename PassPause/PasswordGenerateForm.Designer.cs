namespace Betarium.PassPause
{
    partial class PasswordGenerateForm
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
            this.LengthField = new System.Windows.Forms.TextBox();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.PasswordField = new System.Windows.Forms.TextBox();
            this.CopyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Length";
            // 
            // LengthField
            // 
            this.LengthField.Location = new System.Drawing.Point(76, 12);
            this.LengthField.Name = "LengthField";
            this.LengthField.Size = new System.Drawing.Size(149, 19);
            this.LengthField.TabIndex = 1;
            this.LengthField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LengthField_KeyPress);
            // 
            // GenerateButton
            // 
            this.GenerateButton.Location = new System.Drawing.Point(304, 58);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(75, 23);
            this.GenerateButton.TabIndex = 2;
            this.GenerateButton.Text = "Generate(&G)";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // PasswordField
            // 
            this.PasswordField.Location = new System.Drawing.Point(78, 128);
            this.PasswordField.Name = "PasswordField";
            this.PasswordField.Size = new System.Drawing.Size(303, 19);
            this.PasswordField.TabIndex = 4;
            // 
            // CopyButton
            // 
            this.CopyButton.Location = new System.Drawing.Point(304, 156);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(75, 23);
            this.CopyButton.TabIndex = 5;
            this.CopyButton.Text = "Copy(&C)";
            this.CopyButton.UseVisualStyleBackColor = true;
            this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // PasswordGenerateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 191);
            this.Controls.Add(this.CopyButton);
            this.Controls.Add(this.PasswordField);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.LengthField);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordGenerateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PasswordGenerateForm";
            this.Load += new System.EventHandler(this.PasswordGenerateForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox LengthField;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PasswordField;
        private System.Windows.Forms.Button CopyButton;
    }
}