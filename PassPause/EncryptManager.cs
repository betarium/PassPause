using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Betarium.PassPause
{
    class EncryptManager
    {
        public string EncryptKey { protected get; set; }

        public void MakeKey(string encryptKey, out byte[] aesIV, out byte[] aesKey)
        {
            byte[] encryptKey2 = Encoding.UTF8.GetBytes(encryptKey);

            SHA256 cryptoService = new SHA256CryptoServiceProvider();
            byte[] hashValue = cryptoService.ComputeHash(encryptKey2);
            aesIV = hashValue.Take(16).ToArray();
            aesKey = hashValue.Skip(16).Take(16).ToArray();
        }

        public string EncryptText(string value)
        {
            if (string.IsNullOrEmpty(value))
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

        public string DecryptText(string value)
        {
            if (string.IsNullOrEmpty(value))
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
