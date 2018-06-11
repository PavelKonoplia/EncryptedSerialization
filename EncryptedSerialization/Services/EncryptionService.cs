using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedSerialization.Service
{
    class EncryptionService : IEncryptionService
    {

        private byte[] _key = new byte[16];
        private byte[] _iv = new byte[16];

        public EncryptionService(int key, int iv)
        {
            var tkey = BitConverter.GetBytes(key);
            var tiv = BitConverter.GetBytes(iv);

            int minLength = Math.Min(tkey.Length, tiv.Length);

            int i = 0, k = 0;
            while (i < _key.Length)
            {
                _key[i] = tkey[k];
                _iv[i] = tiv[k];
                i++;
                k++;
                k = k == minLength ? k = 0 : k;
            }
        }

        public byte[] Encrypt(object forEncryption)
        {
            if (forEncryption == null)
                throw new ArgumentNullException("forEncryption");

            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(forEncryption);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        public object Decrypt(byte[] forDecryption)
        {
            if (forDecryption == null)
                throw new ArgumentNullException("forDecryption");

            object decrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(forDecryption))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            decrypted = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return decrypted;
        }
    }
}
