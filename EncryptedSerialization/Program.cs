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
            int[] intArr = new int[] { 2, 3, 4, 5 };
            string[] stringArr = new string[] { "we", "you", "they" };


            BinaryFormatter formatter = new BinaryFormatter();

            SecureMessenger messenger = new SecureMessenger(formatter, "message.txt", "I am your ancestor", "Hello Pipkin!", 23523525, 12.2);
            messenger.Serialization();
            messenger.SendMessage();
          

            SecureMessenger wattsUp = (SecureMessenger)messenger.Deserialization(messenger);

            wattsUp.SendMessage();

            Console.ReadKey();
        }      
    }
}
