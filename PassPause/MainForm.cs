using System;
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
        public bool IsLoaded { get; set; }
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
                if (!LoadFile())
                {
                    Close();
                    return;
                }
                Show();
            }
        }

        private bool LoadFile()
        {
            EncryptManager manager = new EncryptManager();
            manager.EncryptKey = System.Environment.UserName;
            string password2 = manager.DecryptText(Properties.Settings.Default.EncryptKey);

            var configNew = new ConfigAccess();
            configNew.EncryptKey = password2;

            bool success = false;
            FilePath = Path.Combine(ConfigFolder, "Default.xml");
            if (File.Exists(FilePath))
            {
                if (!configNew.Load(FilePath))
                {
                    MessageBox.Show("ファイルの読み込みに失敗しました。");
                    return false;
                }
                else
                {
                    success = true;
                }
            }

            if (!success)
            {
                configNew.Create();
            }

            Config = configNew;

            TreeNode rootNode = FolderTree.Nodes.Add("Default");
            MakeTree(rootNode, "/");
            rootNode.Expand();
            FolderTree.SelectedNode = rootNode;
            IsLoaded = true;
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
                    string backupFile = FilePath + ".bak";
                    if (File.Exists(backupFile))
                    {
                        string backupFile2 = backupFile + "." + DateTime.Now.ToString("yyyyMMdd") + ".bak";
                        if (!File.Exists(backupFile2))
                        {
                            File.Copy(backupFile, backupFile2, true);
                        }
                    }
                    File.Copy(FilePath, backupFile, true);
                }
                Config.Save(FilePath);
            }
        }

        private void FolderTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
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

            if (Config == null)
            {
                return;
            }

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

            if (config.IsRoot)
            {
                Comment.Enabled = true;
                Comment.Text = config.Comment;
                return;
            }

            if (config.IsDirectory)
            {
                AccountName.Enabled = true;
                AccountName.Text = config.Name;
                Comment.Enabled = true;
                Comment.Text = config.Comment;
                return;
            }

            AccountName.Enabled = true;
            AccoutUrl.Enabled = true;
            UserId.Enabled = true;
            Password.Enabled = true;
            Comment.Enabled = true;

            AccountName.Text = config.Name;
            AccoutUrl.Text = config.Url;
            UserId.Text = config.UserId;
            Password.Text = config.Password;
            Comment.Text = config.Comment;
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

            if (config.IsRoot)
            {
                config.Comment = Comment.Text;
                Config.SetItem(itemPath, config);
                return;
            }

            config.Name = AccountName.Text;
            config.Url = AccoutUrl.Text;
            config.UserId = UserId.Text;
            config.Password = Password.Text;
            config.Comment = Comment.Text;

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
                if (!IsLoaded)
                {
                    LoadFile();
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
                if (!IsLoaded)
                {
                    LoadFile();
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

        private void passwordGenerateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasswordGenerateForm form = new PasswordGenerateForm();
            form.Show(this);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
