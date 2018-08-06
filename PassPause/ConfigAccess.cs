using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Betarium.PassPause
{
    public class ConfigAccess
    {
        public class ConfigItem
        {
            public string Name { get; set; }
            public bool IsDirectory { get; set; }
            public bool IsRoot { get; set; }
            public string Url { get; set; }
            public string UserId { get; set; }
            public string Password { get; set; }
            public string Comment { get; set; }
        }

        public enum EncryptModeOption
        {
            None,
            Rot13,
            UrlEncode,
            AES,
        };

        public string FilePath { get; set; }
        public EncryptModeOption EncryptMode { get; set; }
        public string EncryptKey { private get; set; }

        private XmlDocument Document { get; set; }
        private XmlNode RootFolder { get { return Document.SelectSingleNode("//PassPause/RootFolder"); } }

        public ConfigAccess()
        {
            Create();
        }

        public void Create()
        {
            EncryptKey = "";
            EncryptMode = EncryptModeOption.AES;

            Document = new XmlDocument();

            XmlDeclaration declaration = Document.CreateXmlDeclaration("1.0", null, null);
            Document.AppendChild(declaration);

            XmlElement root = Document.CreateElement("PassPause");
            Document.AppendChild(root);

            XmlElement settings = Document.CreateElement("Settings");
            root.AppendChild(settings);

            XmlElement rootFolder = Document.CreateElement("RootFolder");
            root.AppendChild(rootFolder);

            InitSetting(true);
        }

        public bool Load(string filePath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);

            XmlNode rootFolder = xml.SelectSingleNode("//PassPause/RootFolder");
            if (rootFolder == null)
            {
                return false;
            }

            XmlNode settings = xml.SelectSingleNode("//PassPause/Settings");
            if (settings == null)
            {
                return false;
            }

            Document = xml;

            string checkCode = GetSetting("CheckCode");
            string checkHash = GetSetting("CheckHash");

            if (!string.IsNullOrEmpty(checkCode))
            {
                string hash = ConvertCheckCodeToHash(checkCode);

                if (hash != checkHash)
                {
                    return false;
                }
            }

            FilePath = filePath;
            return true;
        }

        public void Save(string filePath)
        {
            InitSetting(false);

            Document.Save(filePath);
        }

        private void InitSetting(bool create)
        {
            SetSetting("FileVersion", "1.0");
            SetSetting("EncryptMode", EncryptMode.ToString());

            string checkCodeOld = GetSetting("CheckCode");

            if (create || string.IsNullOrEmpty(checkCodeOld))
            {
                var checkCode = GetRandamKey();

                string hash = ConvertCheckCodeToHash(checkCode);

                SetSetting("CheckCode", checkCode);
                SetSetting("CheckHash", hash);
            }
        }

        private void SetSetting(string name, string value)
        {
            XmlNode settings = Document.SelectSingleNode("//PassPause/Settings");
            var encryptModeNode = settings.SelectSingleNode(string.Format("item[@name='{0}']", name));
            if (encryptModeNode == null)
            {
                encryptModeNode = Document.CreateElement("item");
                settings.AppendChild(encryptModeNode);
            }
            XmlAttribute attr1 = encryptModeNode.Attributes["name"];
            if (attr1 == null)
            {
                attr1 = Document.CreateAttribute("name");
                encryptModeNode.Attributes.Append(attr1);
            }
            attr1.Value = name;
            XmlAttribute attr2 = encryptModeNode.Attributes["value"];
            if (attr2 == null)
            {
                attr2 = Document.CreateAttribute("value");
                encryptModeNode.Attributes.Append(attr2);
            }
            attr2.Value = value;
        }

        private string GetSetting(string name)
        {
            XmlNode settings = Document.SelectSingleNode("//PassPause/Settings");
            foreach (XmlElement child in settings.ChildNodes)
            {
                if (child.GetAttribute("name") == name)
                {
                    string value = child.GetAttribute("value");
                    return value;
                }
            }

            return null;
        }

        public string JoinPath(string folder, string itemName)
        {
            string currentPath = folder;
            if (currentPath != "/")
            {
                currentPath += "/";
            }
            return currentPath + itemName;
        }

        public ConfigItem GetItem(string path)
        {
            XmlNode item = FindItemNode(path);
            if (item == null)
            {
                return null;
            }

            if (item.Name == "RootFolder")
            {
                ConfigItem data = new ConfigItem();
                data.IsDirectory = true;
                data.IsRoot = true;
                data.Name = "/";
                data.Comment = GetItemComment(item);
                return data;
            }

            XmlAttribute attr1 = item.Attributes["Name"];
            if (attr1 == null)
            {
                return null;
            }

            if (item.Name == "item")
            {
                XmlAttribute attr2 = item.Attributes["Url"];
                XmlAttribute attr3 = item.Attributes["UserId"];
                XmlAttribute attr4 = item.Attributes["Password"];

                ConfigItem data = new ConfigItem();
                data.Name = attr1.Value;
                data.Url = (attr2 != null ? attr2.Value : null);
                data.UserId = (attr3 != null ? attr3.Value : null);
                data.Password = (attr4 != null ? attr4.Value : null);
                data.Comment = GetItemComment(item);

                data.UserId = DecryptText(data.UserId);
                data.Password = DecryptText(data.Password);

                return data;
            }
            else if (item.Name == "folder")
            {
                ConfigItem data = new ConfigItem();
                data.IsDirectory = true;
                data.Name = attr1.Value;
                data.Comment = GetItemComment(item);
                return data;
            }

            return null;
        }

        protected string GetItemComment(XmlNode item)
        {
            foreach (XmlElement child in item.ChildNodes)
            {
                if (child.Name == "comment")
                {
                    return child.InnerText;
                }
            }
            return null;
        }

        public ConfigItem[] GetListData(string folder)
        {
            XmlNode currentNode = FindItemNode(folder);

            List<ConfigItem> list = new List<ConfigItem>();
            foreach (XmlNode item in currentNode)
            {
                XmlAttribute attr1 = item.Attributes["Name"];
                if (attr1 == null)
                {
                    continue;
                }
                string name = attr1.Value;
                string path = JoinPath(folder, name);
                ConfigItem data = GetItem(path);
                list.Add(data);
            }
            return list.ToArray();
        }

        private string GetParentPath(string path)
        {
            string[] pathParts = path.Split('/');
            List<string> newParts = new List<string>();
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                if (i == 0 && pathParts[i] == "")
                {
                    continue;
                }
                newParts.Add(pathParts[i]);
            }
            return "/" + string.Join("/", newParts.ToArray());
        }

        private XmlNode FindItemNode(string path)
        {
            XmlNode rootFolder = RootFolder;

            if (path == "/")
            {
                return rootFolder;
            }

            XmlNode currentNode = rootFolder;
            string[] pathParts = path.Split('/');
            for (int i = 0; i < pathParts.Length; i++)
            {
                string name = pathParts[i];
                if (name == "")
                {
                    continue;
                }
                var folder = currentNode.SelectSingleNode(string.Format("folder[@Name='{0}']", name));
                if (folder != null)
                {
                    currentNode = folder;
                }
                else
                {
                    var node = currentNode.SelectSingleNode(string.Format("item[@Name='{0}']", name));
                    if (node == null)
                    {
                        return null;
                    }
                    if (i + 1 != pathParts.Length)
                    {
                        return null;
                    }
                    return node;
                }
            }
            return currentNode;
        }

        public void SetItem(string path, ConfigItem data)
        {
            System.Diagnostics.Debug.Assert(path != null);
            System.Diagnostics.Debug.Assert(data != null);
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(data.Name));

            string parentFolderPath = GetParentPath(path);
            var parentFolder = FindItemNode(parentFolderPath);
            System.Diagnostics.Debug.Assert(parentFolder != null);
            if (parentFolder == null)
            {
                return;
            }

            var item = FindItemNode(path);
            if (item == null)
            {
                if (data.IsDirectory)
                {
                    item = Document.CreateElement("folder");
                }
                else
                {
                    item = Document.CreateElement("item");
                }
                parentFolder.AppendChild(item);
            }

            XmlAttribute attr1 = Document.CreateAttribute("Name");
            attr1.Value = data.Name;

            item.Attributes.SetNamedItem(attr1);

            if (!data.IsDirectory)
            {
                XmlAttribute attr2 = Document.CreateAttribute("Url");
                attr2.Value = data.Url;
                XmlAttribute attr3 = Document.CreateAttribute("UserId");
                attr3.Value = data.UserId;
                XmlAttribute attr4 = Document.CreateAttribute("Password");
                attr4.Value = data.Password;

                attr3.Value = EncryptText(attr3.Value);
                attr4.Value = EncryptText(attr4.Value);

                item.Attributes.SetNamedItem(attr2);
                item.Attributes.SetNamedItem(attr3);
                item.Attributes.SetNamedItem(attr4);
            }

            SetItemComment(item, data.Comment);
        }

        protected void SetItemComment(XmlNode item, string comment)
        {
            if (!string.IsNullOrEmpty(comment) && item.HasChildNodes)
            {
                var oldList = new List<XmlElement>();
                foreach (XmlElement child in item.ChildNodes)
                {
                    if (child.Name == "comment")
                    {
                        oldList.Add(child);
                    }
                }
                foreach (var child in oldList)
                {
                    item.RemoveChild(child);
                }
            }

            if (!string.IsNullOrEmpty(comment))
            {
                XmlElement commentNode = Document.CreateElement("comment");
                commentNode.InnerText = comment;
                item.AppendChild(commentNode);
            }
        }

        public void RemoveItem(string path)
        {
            var item = FindItemNode(path);
            item.ParentNode.RemoveChild(item);
        }

        public void MoveItemUp(string path)
        {
            var item = FindItemNode(path);
            var parent = item.ParentNode;
            var prev = item.PreviousSibling;
            if (prev == null)
            {
                return;
            }
            parent.InsertBefore(item, prev);
        }

        public void MoveItemDown(string path)
        {
            var item = FindItemNode(path);
            var parent = item.ParentNode;
            var next = item.NextSibling;
            if (next == null)
            {
                return;
            }
            parent.InsertAfter(item, next);
        }

        private void MakeKey(string encryptKey, out byte[] aesIV, out byte[] aesKey)
        {
            byte[] encryptKey2 = Encoding.UTF8.GetBytes(encryptKey);

            SHA256 cryptoService = new SHA256CryptoServiceProvider();
            byte[] hashValue = cryptoService.ComputeHash(encryptKey2);
            aesIV = hashValue.Take(16).ToArray();
            aesKey = hashValue.Skip(16).Take(16).ToArray();
        }

        private string EncryptText(string value)
        {
            if (EncryptMode == EncryptModeOption.AES)
            {
                value = EncryptTextAes(value);
            }
            else if (EncryptMode == EncryptModeOption.UrlEncode)
            {
                value = Uri.EscapeDataString(value);
            }
            return value;
        }

        private string EncryptTextAes(string value)
        {
            if (value == null)
            {
                return null;
            }

            byte[] aesIV, aesKey;
            MakeKey(EncryptKey, out aesIV, out aesKey);

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = aesIV;
            aes.Key = aesKey;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] src = Encoding.UTF8.GetBytes(value);

            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);
                string result = Convert.ToBase64String(dest);
                return result;
            }
        }

        private string DecryptText(string value)
        {
            if (EncryptMode == EncryptModeOption.AES)
            {
                value = DecryptTextAes(value);
            }
            else if (EncryptMode == EncryptModeOption.UrlEncode)
            {
                value = Uri.UnescapeDataString(value);
            }
            return value;
        }

        private string DecryptTextAes(string value)
        {
            if (value == null)
            {
                return null;
            }

            byte[] aesIV, aesKey;
            MakeKey(EncryptKey, out aesIV, out aesKey);

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = aesIV;
            aes.Key = aesKey;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] src = System.Convert.FromBase64String(value);

            using (ICryptoTransform decrypt = aes.CreateDecryptor())
            {
                byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                string result = Encoding.UTF8.GetString(dest);
                return result;
            }
        }

        private string GetRandamKey()
        {
            Random rand = new Random();
            string result = "";
            for (int i = 0; i < 20; i++)
            {
                result += Char.ConvertFromUtf32('A' + rand.Next() % 26);
            }
            return result;
        }

        private string ConvertCheckCodeToHash(string checkCode)
        {
            string hash1 = EncryptText(checkCode);

            SHA256 cryptoService = new SHA256CryptoServiceProvider();
            byte[] hash2 = Encoding.UTF8.GetBytes(hash1);
            byte[] hash3 = cryptoService.ComputeHash(hash2);
            string hash4 = Convert.ToBase64String(hash3);

            return hash4;
        }
    }
}
