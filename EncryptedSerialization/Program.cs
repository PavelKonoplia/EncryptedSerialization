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

            BinaryFormatter formatter = new BinaryFormatter();

            SecureMessenger messenger = new SecureMessenger(formatter, "message.txt", "Hello Pipkin!");

            messenger.Serialization();
            messenger.SendMessage();

            SecureMessenger wattsUp = (SecureMessenger)messenger.Deserialization(messenger);

            wattsUp.SendMessage();

            Console.ReadKey();
        }
    }
}
