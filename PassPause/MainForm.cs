using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Betarium.PassPause
{
    public partial class MainForm : Form
    {
        public string FilePath { get; set; }
        public ConfigAccess Config { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            AccountName.ReadOnly = false;
            Comment.Enabled = false;

            FolderTree.ShowRootLines = false;
            TreeNode rootNode = FolderTree.Nodes.Add("Default");
            //TreeNode rootNode2 = FolderTree.Nodes.Add("Share");

            if (string.IsNullOrEmpty(Properties.Settings.Default.EncryptKey))
            {
                Random rand = new Random();
                byte[] buf = new byte[16];
                for (int i = 0; i < 16; i++)
                {
                    buf[i] = (byte)rand.Next(256);
                }
                string encryptKey = Convert.ToBase64String(buf);

                Properties.Settings.Default.EncryptKey = encryptKey;
                Properties.Settings.Default.Save();
            }

            FilePath = Path.Combine(Application.StartupPath, "PassPause.xml");
            Config = new ConfigAccess();
            Config.EncryptKey = Properties.Settings.Default.EncryptKey;

            if (File.Exists(FilePath))
            {
                Config.Load(FilePath);
            }

            MakeTree(rootNode, "/");
            rootNode.Expand();
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
            SaveItem();
            if (File.Exists(FilePath))
            {
                File.Copy(FilePath, FilePath + "." + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak", true);
            }
            Config.Save(FilePath);
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

            bool IsRoot = (currentNode != null);
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

            currentNode.Remove();
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
            FolderTree.SelectedNode = currentNode;

            string currentPath = GetCurrentPath();
            Config.MoveItemDown(currentPath);
        }
    }
}
