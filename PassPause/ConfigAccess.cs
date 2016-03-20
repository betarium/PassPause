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
            public string Url { get; set; }
            public string UserId { get; set; }
            public string Password { get; set; }
            public string Comment { get; set; }
        }

        public string EncryptKey { private get; set; }

        private XmlDocument Document { get; set; }

        public ConfigAccess()
        {
            Create();
        }

        public void Create()
        {
            EncryptKey = "";
            Document = new XmlDocument();

            XmlDeclaration declaration = Document.CreateXmlDeclaration("1.0", null, null);
            Document.AppendChild(declaration);

            XmlElement toptag = Document.CreateElement("PassPause");
            Document.AppendChild(toptag);

            XmlElement root = Document.CreateElement("RootFolder");
            toptag.AppendChild(root);
        }

        public void Load(string filePath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);

            XmlNode root = xml.SelectSingleNode("//PassPause/RootFolder");
            if (root == null)
            {
                return;
            }

            Document = xml;
        }

        public void Save(string filePath)
        {
            Document.Save(filePath);
        }

        protected XmlNode RootFolder { get { return Document.SelectSingleNode("//PassPause/RootFolder"); } }

        /*
        public ConfigData FindData(string name)
        {
            XmlNode root = Document.SelectSingleNode("//PassPause/RootFolder");

            ConfigData data = new ConfigData();
            foreach (XmlNode item in root)
            {
                XmlAttribute attr1 = item.Attributes["Name"];
                XmlAttribute attr2 = item.Attributes["Url"];
                XmlAttribute attr3 = item.Attributes["UserId"];
                XmlAttribute attr4 = item.Attributes["Password"];
                if (attr1 == null)
                {
                    continue;
                }
                if (attr1.Name == name)
                {
                    return data;
                }
            }
            return null;
        }
        */

        public string JoinPath(string folder, string itemName)
        {
            string currentPath = folder;
            if (currentPath != "/")
            {
                currentPath += "/";
            }
            return currentPath + itemName;
        }

        public ConfigItem GetItemData(string path)
        {
            XmlNode item = FindItemNode(path);
            if (item == null)
            {
                return null;
            }

            if (item.Name == "item")
            {
                XmlAttribute attr1 = item.Attributes["Name"];
                XmlAttribute attr2 = item.Attributes["Url"];
                XmlAttribute attr3 = item.Attributes["UserId"];
                XmlAttribute attr4 = item.Attributes["Password"];
                if (attr1 == null)
                {
                    return null;
                }

                ConfigItem data = new ConfigItem();
                data.Name = attr1.Value;
                data.Url = (attr2 != null ? attr2.Value : null);
                data.UserId = (attr3 != null ? attr3.Value : null);
                data.Password = (attr4 != null ? attr4.Value : null);

                data.UserId = DecryptText(data.UserId);
                data.Password = DecryptText(data.Password);

                return data;
            }
            else if (item.Name == "folder")
            {
                XmlAttribute attr1 = item.Attributes["Name"];
                if (attr1 == null)
                {
                    return null;
                }
                ConfigItem data = new ConfigItem();
                data.IsDirectory = true;
                data.Name = attr1.Value;
                return data;
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
                string name = attr1.Value;
                string path = JoinPath(folder, name);
                ConfigItem data = GetItemData(path);
                list.Add(data);
            }
            return list.ToArray();
        }

        /*
        public ConfigItem FindData(string path)
        {
            string[] pathItem = path.Split('\\');
            string name = pathItem.Last();
            ConfigItem[] list = GetListData(path);

            foreach (var item in list)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }
        */

        /*
        public void AddData(ConfigData data)
        {
            XmlElement item = Document.CreateElement("item");
            XmlNode folder = Document.SelectSingleNode("//PassPause/RootFolder");
            folder.AppendChild(item);

            XmlAttribute attr1 = Document.CreateAttribute("Name");
            attr1.Value = data.Name;
            XmlAttribute attr2 = Document.CreateAttribute("Url");
            attr2.Value = data.Url;
            XmlAttribute attr3 = Document.CreateAttribute("UserId");
            attr3.Value = data.UserId;
            XmlAttribute attr4 = Document.CreateAttribute("Password");
            attr4.Value = data.Password;

            item.Attributes.Append(attr1);
            item.Attributes.Append(attr2);
            item.Attributes.Append(attr3);
            item.Attributes.Append(attr4);

            if (!string.IsNullOrEmpty(data.Comment))
            {
                XmlElement comment = Document.CreateElement("comment");
                comment.InnerText = data.Comment;
                item.AppendChild(comment);
            }
        }
         */

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
                item = Document.CreateElement("item");
                if (data.IsDirectory)
                {
                    item = Document.CreateElement("folder");
                }
                parentFolder.AppendChild(item);
            }

            if (data.IsDirectory)
            {
                XmlAttribute attr1 = Document.CreateAttribute("Name");
                attr1.Value = data.Name;

                item.Attributes.SetNamedItem(attr1);
            }
            else
            {
                XmlAttribute attr1 = Document.CreateAttribute("Name");
                attr1.Value = data.Name;
                XmlAttribute attr2 = Document.CreateAttribute("Url");
                attr2.Value = data.Url;
                XmlAttribute attr3 = Document.CreateAttribute("UserId");
                attr3.Value = data.UserId;
                XmlAttribute attr4 = Document.CreateAttribute("Password");
                attr4.Value = data.Password;

                attr3.Value = EncryptText(attr3.Value);
                attr4.Value = EncryptText(attr4.Value);

                item.Attributes.SetNamedItem(attr1);
                item.Attributes.SetNamedItem(attr2);
                item.Attributes.SetNamedItem(attr3);
                item.Attributes.SetNamedItem(attr4);

                if (!string.IsNullOrEmpty(data.Comment))
                {
                    XmlElement comment = Document.CreateElement("comment");
                    comment.InnerText = data.Comment;
                    item.AppendChild(comment);
                }
            }
        }

        public void RemoveItem(string path)
        {
            var item = FindItemNode(path);
            item.ParentNode.RemoveChild(item);
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

    }
}
