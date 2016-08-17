﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Betarium.PassPause
{
    public partial class MainForm : Form
    {
        public string ConfigFolder { get; set; }
        public bool IsLogin { get; set; }
        public string FilePath { get; set; }
        public ConfigAccess Config { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Comment.Enabled = false;

            FolderTree.ShowRootLines = false;

            TrayIcon.Text = Application.ProductName;
#if DEBUG
            TrayIcon.Text = TrayIcon.Text + " (DEBUG)";
#endif

            if (string.IsNullOrEmpty(Properties.Settings.Default.EncryptKey))
            {
                Show();

                if (!InitPassword())
                {
                    Close();
                    return;
                }

                //Random rand = new Random();
                //byte[] buf = new byte[16];
                //for (int i = 0; i < 16; i++)
                //{
                //    buf[i] = (byte)rand.Next(256);
                //}
                //string encryptKey = Convert.ToBase64String(buf);

                //Properties.Settings.Default.EncryptKey = encryptKey;
                //Properties.Settings.Default.Save();
            }

            string documentFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string configFolder = Path.Combine(documentFolder, Application.ProductName);
            if (!Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }
            ConfigFolder = configFolder;

            if (WindowState != FormWindowState.Minimized)
            {
                if (!Login())
                {
                    Close();
                    return;
                }
                Show();
            }
        }

        private bool Login()
        {
            var loginForm = new LoginForm();
            loginForm.UserIdField.Text = Environment.UserName;
            loginForm.Auth += LoginForm_Auth;
            if (loginForm.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            EncryptManager manager = new EncryptManager();
            manager.EncryptKey = System.Environment.UserName;
            string password2 = manager.DecryptText(Properties.Settings.Default.EncryptKey);

            Config = new ConfigAccess();
            Config.EncryptKey = password2;

            FilePath = Path.Combine(ConfigFolder, "Default.xml");
            if (File.Exists(FilePath))
            {
                Config.Load(FilePath);
            }

            TreeNode rootNode = FolderTree.Nodes.Add("Default");
            MakeTree(rootNode, "/");
            rootNode.Expand();
            IsLogin = true;
            return true;
        }

        private void LoginForm_Auth(LoginForm.AuthEventArgs e)
        {
            if (e.UserId.Length == 0 || e.Password.Length == 0)
            {
                return;
            }

            string userId = Properties.Settings.Default.LoginUserId;
            string password = Properties.Settings.Default.LoginPassword;
            if (userId.Length == 0)
            {
                Properties.Settings.Default.LoginUserId = e.UserId;
                Properties.Settings.Default.LoginPassword = e.Password;
                Properties.Settings.Default.Save();
                e.Success = true;
                return;
            }
            if (e.UserId != userId || e.Password != password)
            {
                return;
            }
            e.Success = true;
        }

        private bool InitPassword()
        {
            PasswordForm passwordForm = new PasswordForm();
            if (passwordForm.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
            {
                return false;
            }

            string password = passwordForm.Password;

            EncryptManager manager = new EncryptManager();
            manager.EncryptKey = System.Environment.UserName;
            string password2 = manager.EncryptText(password);

            Properties.Settings.Default.EncryptKey = password2;
            Properties.Settings.Default.Save();
            return true;
        }

        private void MakeTree(TreeNode currentNode, string path)
        {
            var list = Config.GetListData(path);
            foreach (var configData in list)
            {
                var node = currentNode.Nodes.Add(configData.Name);
                node.Name = configData.Name;
                //node.Tag = configData;
                if (configData.IsDirectory)
                {
                    TreeNode[] itemNodeList = currentNode.Nodes.Find(configData.Name, false);
                    if (itemNodeList.Length > 0)
                    {
                        MakeTree(itemNodeList[0], Config.JoinPath(path, configData.Name));
                    }
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Config != null)
            {
                SaveItem();
                if (File.Exists(FilePath))
                {
                    File.Copy(FilePath, FilePath + "." + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak", true);
                }
                Config.Save(FilePath);
            }
        }

        private void FolderTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (Config == null)
            {
                return;
            }

            AccountName.Text = "";
            AccoutUrl.Text = "";
            UserId.Text = "";
            Password.Text = "";
            Comment.Text = "";

            AccountName.Enabled = false;
            AccoutUrl.Enabled = false;
            UserId.Enabled = false;
            Password.Enabled = false;
            Comment.Enabled = false;

            var node = FolderTree.SelectedNode;
            if (node == null)
            {
                return;
            }

            string currentPath = GetCurrentPath();
            var config = Config.GetItem(currentPath);

            if (config == null)
            {
                return;
            }

            if (config.IsDirectory)
            {
                AccountName.Enabled = true;
                AccountName.Text = config.Name;
                return;
            }

            AccountName.Enabled = true;
            AccoutUrl.Enabled = true;
            UserId.Enabled = true;
            Password.Enabled = true;
            //Comment.Enabled = true;

            AccountName.Text = config.Name;
            AccoutUrl.Text = config.Url;
            UserId.Text = config.UserId;
            Password.Text = config.Password;
        }

        private void FolderTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            var node = FolderTree.SelectedNode;
            if (node == null)
            {
                return;
            }

            SaveItem();
        }

        private void SaveItem()
        {
            var node = FolderTree.SelectedNode;
            if (node == null)
            {
                return;
            }

            string itemPath = GetCurrentPath();
            var config = Config.GetItem(itemPath);
            if (config == null)
            {
                return;
            }
            config.Name = AccountName.Text;
            config.Url = AccoutUrl.Text;
            config.UserId = UserId.Text;
            config.Password = Password.Text;

            if (string.IsNullOrEmpty(config.Name))
            {
                return;
            }

            Config.SetItem(itemPath, config);

            if (node.Name != config.Name)
            {
                node.Name = config.Name;
                node.Text = config.Name;
            }
        }

        private void addMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentNode = FolderTree.SelectedNode;
            if (currentNode == null)
            {
                return;
            }

            string itemPath = GetCurrentPath();

            string itemName = null;
            for (int i = 1; i < 1000; i++)
            {
                itemName = "item" + i.ToString();
                var oldData = Config.GetItem(itemPath + "/" + itemName);
                if (oldData == null)
                {
                    break;
                }
            }
            if (itemName == null)
            {
                MessageBox.Show("これ以上追加できません。");
                return;
            }

            ConfigAccess.ConfigItem configData = new ConfigAccess.ConfigItem();
            configData.Name = itemName;

            Config.SetItem(Config.JoinPath(itemPath, itemName), configData);

            var node = currentNode.Nodes.Add(itemName);
            node.Name = itemName;
            //node.Tag = configData;
            FolderTree.SelectedNode = node;
        }

        private void FolderTreeMenu_Opening(object sender, CancelEventArgs e)
        {
            var currentNode = FolderTree.SelectedNode;

            string path = GetCurrentPath();
            var item = Config.GetItem(path);

            bool IsRoot = (FolderTree.TopNode == currentNode || FolderTree.TopNode == currentNode.Parent);
            bool IsItem = (currentNode != null) && (item != null) && (!item.IsDirectory);
            bool IsDirectory = (currentNode != null) && (item != null) && (item.IsDirectory);

            addFolderMenuToolStripMenuItem.Enabled = (IsRoot || IsDirectory);
            addMenuToolStripMenuItem.Enabled = (IsRoot || IsDirectory);
            deleteToolStripMenuItem.Enabled = (IsDirectory || IsItem);
        }

        private string GetCurrentPath()
        {
            var currentNode = FolderTree.SelectedNode;
            if (currentNode == null)
            {
                return null;
            }

            string path = currentNode.FullPath;
            path = path.Replace('\\', '/');
            if (path.IndexOf('/') < 0)
            {
                return "/";
            }
            path = path.Substring(path.IndexOf('/'));
            return path;
        }

        private string JoinPath(string folder, string itemName)
        {
            string currentPath = folder;
            if (currentPath != "/")
            {
                currentPath += "/";
            }
            return currentPath + itemName;
        }

        private void addFolderMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentNode = FolderTree.SelectedNode;
            if (currentNode == null)
            {
                return;
            }

            string folderPath = GetCurrentPath();

            string itemName = null;
            for (int i = 1; i < 1000; i++)
            {
                itemName = "folder" + i.ToString();
                var oldData = Config.GetItem(JoinPath(folderPath, itemName));
                if (oldData == null)
                {
                    break;
                }
            }
            if (itemName == null)
            {
                MessageBox.Show("これ以上追加できません。");
                return;
            }

            ConfigAccess.ConfigItem configData = new ConfigAccess.ConfigItem();
            configData.Name = itemName;
            configData.IsDirectory = true;

            Config.SetItem(JoinPath(folderPath, itemName), configData);

            var node = currentNode.Nodes.Add(itemName);
            node.Name = itemName;
            //node.Tag = configData;
            FolderTree.SelectedNode = node;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentNode = FolderTree.SelectedNode;
            if (currentNode == null)
            {
                return;
            }

            if (currentNode.Parent == null)
            {
                return;
            }

            string currentPath = GetCurrentPath();
            Config.RemoveItem(currentPath);
            currentNode.Remove();
        }

        private void CopyUserIdButton_Click(object sender, EventArgs e)
        {
            var currentNode = FolderTree.SelectedNode;
            if (currentNode == null)
            {
                return;
            }

            string value = UserId.Text;
            if (string.IsNullOrEmpty(value))
            {
                Clipboard.Clear();
                return;
            }
            Clipboard.SetText(value);
        }

        private void CopyPasswordButton_Click(object sender, EventArgs e)
        {
            var currentNode = FolderTree.SelectedNode;
            if (currentNode == null)
            {
                return;
            }

            string value = Password.Text;
            if (string.IsNullOrEmpty(value))
            {
                Clipboard.Clear();
                return;
            }
            Clipboard.SetText(value);
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentNode = FolderTree.SelectedNode;
            if (currentNode == null)
            {
                return;
            }

            TreeNode parent = currentNode.Parent;
            if (parent == null)
            {
                return;
            }

            TreeNode prev = currentNode.PrevNode;
            if (prev == null)
            {
                return;
            }

            int oldIndex = currentNode.Index;
            currentNode.Remove();
            parent.Nodes.Insert(oldIndex - 1, currentNode);

            /*
            List<TreeNode> nodeList = new List<TreeNode>();
            foreach (TreeNode tmp in parent.Nodes)
            {
                nodeList.Add(tmp);
            }

            parent.Nodes.Clear();

            for (int i = 0; i < nodeList.Count; i++)
            {
                if (currentNode.Index - 1 == i)
                {
                    parent.Nodes.Add(currentNode);
                }
                parent.Nodes.Add(nodeList[i]);
            }
            */

            FolderTree.SelectedNode = currentNode;

            string currentPath = GetCurrentPath();
            Config.MoveItemUp(currentPath);
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentNode = FolderTree.SelectedNode;
            if (currentNode == null)
            {
                return;
            }

            TreeNode parent = currentNode.Parent;
            if (parent == null)
            {
                return;
            }

            TreeNode next = currentNode.NextNode;
            if (next == null)
            {
                return;
            }

            int oldIndex = currentNode.Index;
            currentNode.Remove();
            parent.Nodes.Insert(oldIndex + 1, currentNode);

            /*
            currentNode.Remove();
            List<TreeNode> nodeList = new List<TreeNode>();
            foreach (TreeNode tmp in parent.Nodes)
            {
                nodeList.Add(tmp);
            }

            parent.Nodes.Clear();

            bool nodeAdd = false;
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (!nodeAdd && currentNode.Index + 1 == i)
                {
                    parent.Nodes.Add(currentNode);
                    nodeAdd = true;
                }
                parent.Nodes.Add(nodeList[i]);
            }
            if (!nodeAdd)
            {
                parent.Nodes.Add(currentNode);
            }
            */

            FolderTree.SelectedNode = currentNode;

            string currentPath = GetCurrentPath();
            Config.MoveItemDown(currentPath);
        }

        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Show();
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }
                if (!IsLogin)
                {
                    Login();
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Close();
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && Visible)
            {
                Visible = false;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized && Visible)
            {
                if (!IsLogin)
                {
                    Login();
                }
            }

            if (WindowState == FormWindowState.Minimized && Visible)
            {
                Visible = false;
            }
        }

        private void OpenLinkButton_Click(object sender, EventArgs e)
        {
            var currentNode = FolderTree.SelectedNode;
            if (currentNode == null)
            {
                return;
            }

            string value = AccoutUrl.Text;
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            if (value.StartsWith("rdp:"))
            {
                string path = Regex.Replace(value, "^rdp:", "");
                string command = Environment.ExpandEnvironmentVariables(@"%windir%\system32\mstsc.exe");
                using (var process = Process.Start(command, "/v:" + path)) { }
                return;
            }
            try
            {
                using (var process = Process.Start(value)) { }
            }
            catch (Exception ex)
            {
                MessageBox.Show("実行できません。" + ex.Message, Application.ProductName);
            }
        }

        private void CopyLinkButton_Click(object sender, EventArgs e)
        {
            var currentNode = FolderTree.SelectedNode;
            if (currentNode == null)
            {
                return;
            }

            string value = AccoutUrl.Text;
            if (string.IsNullOrEmpty(value))
            {
                Clipboard.Clear();
                return;
            }
            Clipboard.SetText(value);
        }
    }
}
