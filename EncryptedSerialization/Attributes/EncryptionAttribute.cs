using EncryptedSerialization.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedSerialization.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    class EncryptionAttribute : Attribute
    {
        private byte[] _key = new byte[16];
        private byte[] _iv = new byte[16];

        public IEncryptionService EncryptionService;
        public byte[] Key { get { return _key; }}
        public byte[] IV { get { return _iv; } }

        public EncryptionAttribute(string serviceName, int key, int iv)
        {
            SetService(serviceName);

            var tkey = BitConverter.GetBytes(key);
            var tiv = BitConverter.GetBytes(iv);

            int i = 0, k =0;
            while (i< _key.Length)
            {
                _key[i] = tkey[k];
                _iv[i] = tiv[k];
                i++;
                k++;
                k = k == 4 ? k = 0 : k;
            }
        }

        private void SetService(string serviceName) {

            try
            {
                Type service = Assembly.GetExecutingAssembly().GetType(serviceName);
                EncryptionService = (IEncryptionService)Activator.CreateInstance(service);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
