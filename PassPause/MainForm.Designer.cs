namespace Betarium.PassPause
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FolderTreeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFolderMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.FolderTree = new System.Windows.Forms.TreeView();
            this.CopyPasswordButton = new System.Windows.Forms.Button();
            this.CopyUserIdButton = new System.Windows.Forms.Button();
            this.Comment = new System.Windows.Forms.TextBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.UserId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AccoutUrl = new System.Windows.Forms.TextBox();
            this.UrlField = new System.Windows.Forms.Label();
            this.AccountName = new System.Windows.Forms.TextBox();
            this.FolderTreeMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TrayIcon
            // 
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Visible = true;
            this.TrayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(504, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FolderTreeMenu
            // 
            this.FolderTreeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMenuToolStripMenuItem,
            this.addFolderMenuToolStripMenuItem,
            this.toolStripSeparator1,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.toolStripSeparator2,
            this.deleteToolStripMenuItem});
            this.FolderTreeMenu.Name = "contextMenuStrip1";
            this.FolderTreeMenu.Size = new System.Drawing.Size(163, 126);
            this.FolderTreeMenu.Opening += new System.ComponentModel.CancelEventHandler(this.FolderTreeMenu_Opening);
            // 
            // addMenuToolStripMenuItem
            // 
            this.addMenuToolStripMenuItem.Name = "addMenuToolStripMenuItem";
            this.addMenuToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.addMenuToolStripMenuItem.Text = "アイテムを追加(&A)";
            this.addMenuToolStripMenuItem.Click += new System.EventHandler(this.addMenuToolStripMenuItem_Click);
            // 
            // addFolderMenuToolStripMenuItem
            // 
            this.addFolderMenuToolStripMenuItem.Name = "addFolderMenuToolStripMenuItem";
            this.addFolderMenuToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.addFolderMenuToolStripMenuItem.Text = "フォルダを追加(&F)";
            this.addFolderMenuToolStripMenuItem.Click += new System.EventHandler(this.addFolderMenuToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(159, 6);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.moveUpToolStripMenuItem.Text = "上に移動(&U)";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.moveDownToolStripMenuItem.Text = "下に移動(&N)";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(159, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.deleteToolStripMenuItem.Text = "削除(&D)";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 306);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(504, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.FolderTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.CopyPasswordButton);
            this.splitContainer1.Panel2.Controls.Add(this.CopyUserIdButton);
            this.splitContainer1.Panel2.Controls.Add(this.Comment);
            this.splitContainer1.Panel2.Controls.Add(this.Password);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.UserId);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.AccoutUrl);
            this.splitContainer1.Panel2.Controls.Add(this.UrlField);
            this.splitContainer1.Panel2.Controls.Add(this.AccountName);
            this.splitContainer1.Size = new System.Drawing.Size(504, 282);
            this.splitContainer1.SplitterDistance = 168;
            this.splitContainer1.TabIndex = 1;
            // 
            // FolderTree
            // 
            this.FolderTree.ContextMenuStrip = this.FolderTreeMenu;
            this.FolderTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FolderTree.HideSelection = false;
            this.FolderTree.Location = new System.Drawing.Point(0, 0);
            this.FolderTree.Name = "FolderTree";
            this.FolderTree.Size = new System.Drawing.Size(168, 282);
            this.FolderTree.TabIndex = 0;
            this.FolderTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.FolderTree_BeforeSelect);
            this.FolderTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FolderTree_AfterSelect);
            // 
            // CopyPasswordButton
            // 
            this.CopyPasswordButton.Location = new System.Drawing.Point(263, 100);
            this.CopyPasswordButton.Name = "CopyPasswordButton";
            this.CopyPasswordButton.Size = new System.Drawing.Size(57, 23);
            this.CopyPasswordButton.TabIndex = 8;
            this.CopyPasswordButton.Text = "Copy";
            this.CopyPasswordButton.UseVisualStyleBackColor = true;
            this.CopyPasswordButton.Click += new System.EventHandler(this.CopyPasswordButton_Click);
            // 
            // CopyUserIdButton
            // 
            this.CopyUserIdButton.Location = new System.Drawing.Point(263, 75);
            this.CopyUserIdButton.Name = "CopyUserIdButton";
            this.CopyUserIdButton.Size = new System.Drawing.Size(57, 23);
            this.CopyUserIdButton.TabIndex = 5;
            this.CopyUserIdButton.Text = "Copy";
            this.CopyUserIdButton.UseVisualStyleBackColor = true;
            this.CopyUserIdButton.Click += new System.EventHandler(this.CopyUserIdButton_Click);
            // 
            // Comment
            // 
            this.Comment.Location = new System.Drawing.Point(15, 136);
            this.Comment.Multiline = true;
            this.Comment.Name = "Comment";
            this.Comment.Size = new System.Drawing.Size(305, 132);
            this.Comment.TabIndex = 9;
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(90, 102);
            this.Password.Name = "Password";
            this.Password.Size = new System.Drawing.Size(167, 19);
            this.Password.TabIndex = 7;
            this.Password.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Password";
            // 
            // UserId
            // 
            this.UserId.Location = new System.Drawing.Point(90, 77);
            this.UserId.Name = "UserId";
            this.UserId.Size = new System.Drawing.Size(167, 19);
            this.UserId.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "User ID";
            // 
            // AccoutUrl
            // 
            this.AccoutUrl.Location = new System.Drawing.Point(90, 41);
            this.AccoutUrl.Name = "AccoutUrl";
            this.AccoutUrl.Size = new System.Drawing.Size(230, 19);
            this.AccoutUrl.TabIndex = 2;
            // 
            // UrlField
            // 
            this.UrlField.AutoSize = true;
            this.UrlField.Location = new System.Drawing.Point(13, 44);
            this.UrlField.Name = "UrlField";
            this.UrlField.Size = new System.Drawing.Size(27, 12);
            this.UrlField.TabIndex = 1;
            this.UrlField.Text = "URL";
            // 
            // AccountName
            // 
            this.AccountName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountName.Location = new System.Drawing.Point(15, 12);
            this.AccountName.Name = "AccountName";
            this.AccountName.Size = new System.Drawing.Size(305, 19);
            this.AccountName.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 328);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "PassPause";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.FolderTreeMenu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ContextMenuStrip FolderTreeMenu;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView FolderTree;
        private System.Windows.Forms.TextBox Comment;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox UserId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AccoutUrl;
        private System.Windows.Forms.Label UrlField;
        private System.Windows.Forms.TextBox AccountName;
        private System.Windows.Forms.ToolStripMenuItem addMenuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFolderMenuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button CopyPasswordButton;
        private System.Windows.Forms.Button CopyUserIdButton;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

