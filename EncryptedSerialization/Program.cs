using EncryptedSerialization.Logic;
using EncryptedSerialization.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
          //  Example();

            int[] intArr = new int[] { 2, 3, 4, 5 };
            string[] stringArr = new string[] { "we", "you", "they" };
            List<ForTest> testList = new List<ForTest>();
            testList.Add(new ForTest("Test 1"));
            testList.Add(new ForTest("Test 2"));
            testList.Add(new ForTest("Test 3"));


            BinaryFormatter formatter = new BinaryFormatter();

            SecureMessenger messenger = new SecureMessenger(formatter, "message.txt", "I am your ancestor", "Hello Pipkin!", 23523525, 12.2, new ForTest("Test 1"));
            messenger.Serialization();
            messenger.SendMessage();
            messenger.Test.PrintTest();
          

            SecureMessenger wattsUp = (SecureMessenger)messenger.Deserialization(messenger);

            wattsUp.SendMessage();

            Console.ReadKey();
        }

        public static void Example()
        {
            byte[] _key = new byte[16];
            byte[] _iv = new byte[16];
            EncryptionService es = new EncryptionService();
            ForTest ft = new ForTest("Test 1");

            var tkey = BitConverter.GetBytes(1111);
            var tiv = BitConverter.GetBytes(1111);

            int i = 0, k = 0;
            while (i < 16)
            {
                _key[i] = tkey[k];
                _iv[i] = tiv[k];
                i++;
                k++;
                k = k == 4 ? k = 0 : k;
            }
            ft.PrintTest();
            byte[] shifr = es.Encrypt(ft, _key, _iv);
            ForTest fdesc = es.Decrypt(shifr, _key, _iv) as ForTest;
            fdesc.PrintTest();

        }
    }
}
