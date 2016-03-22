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
    public partial class LoginForm : Form
    {
        public class AuthEventArgs : EventArgs
        {
            public string UserId { get; set; }
            public string Password { get; set; }
            public bool Success { get; set; }
        }

        public delegate void AuthEventHandler(AuthEventArgs e);

        public event AuthEventHandler Auth;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            if (UserIdField.Text == "")
            {
                MessageBox.Show("ユーザー名を入力してください");
                UserIdField.Focus();
                return;
            }
            else if (PasswordField.Text == "")
            {
                MessageBox.Show("パスワードを入力してください");
                PasswordField.Focus();
                return;
            }

            AuthEventArgs e2 = new AuthEventArgs();
            e2.UserId = UserIdField.Text;
            e2.Password = PasswordField.Text;
            Auth(e2);
            if (!e2.Success)
            {
                MessageBox.Show("ユーザー名またはパスワードに誤りがあります");
                UserIdField.Focus();
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            Activate();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
