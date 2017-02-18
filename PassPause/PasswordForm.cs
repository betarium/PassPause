using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Betarium.PassPause
{
    public partial class PasswordForm : Form
    {
        public string Password { get; set; }

        public PasswordForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordField.Text))
            {
                MessageBox.Show("パスワードを入力してください。");
                return;
            }
            else if (PasswordField.Text != PasswordConfirmField.Text)
            {
                MessageBox.Show("パスワード（確認）と一致しません。");
                return;
            }
            Password = PasswordField.Text;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
