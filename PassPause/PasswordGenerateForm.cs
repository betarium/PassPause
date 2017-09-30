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
    public partial class PasswordGenerateForm : Form
    {
        private PasswordGenerateManager GenerateManager = new PasswordGenerateManager();

        public PasswordGenerateForm()
        {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            int length = 8;
            if (!int.TryParse(LengthField.Text, out length))
            {
                MessageBox.Show("数字で入力してください。");
                return;
            }
            string password = GenerateManager.Generate(length);
            PasswordField.Text = password;
        }

        private void PasswordGenerateForm_Load(object sender, EventArgs e)
        {
            LengthField.Text = "8";
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            if (PasswordField.Text.Length == 0)
            {
                return;
            }
            Clipboard.SetText(PasswordField.Text);
        }

        private void LengthField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
        }
    }
}
