using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.Services
{
    public class EncryptService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public EncryptService(string secretKey)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(secretKey));
            }
            _iv = new byte[16];
        }

        public string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(plainText), 0, plainText.Length);
                return Convert.ToBase64String(encryptedBytes);

            }
        }

        public string Decrypt(string encryptedText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }

}
