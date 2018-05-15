using EncryptedSerialization.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedSerialization.Logic
{
    [Serializable]
    public class ForTest
    {
        public string Test { get; set; }
        public ForTest(string s)
        {
            Test = s;
        }
        public void PrintTest()
        {
            Console.WriteLine(Test);
        }
    }

    [Serializable]
    public class SecureMessenger : EncryptionSerializator
    {

        public string Message { get; set; }
        public int MessageID { get; set; }
        public double MessagePoints { get; set; }
        public ForTest Test { get; set; }

        public SecureMessenger() { }
        public SecureMessenger(IFormatter formatter, string filePath,string ancient, string message, int messageID, double messagePoints, ForTest test) : base(formatter, filePath, ancient)
        {
            Message = message;
            MessageID = messageID;
            MessagePoints = messagePoints;
            Test = test;
        }
        public SecureMessenger(IFormatter formatter, string filePath, FileMode fileMode, string ancient,string message, int messageID, double messagePoints, ForTest test) : base(formatter, filePath, ancient, fileMode)
        {
            Message = message;
            MessageID = messageID;
            MessagePoints = messagePoints;
            Test = test;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Message", EncryptionService.Encrypt(Message, Key, IV));
            info.AddValue("MessageID", EncryptionService.Encrypt(MessageID, Key, IV));
            info.AddValue("MessagePoints", EncryptionService.Encrypt(MessagePoints, Key, IV));
            info.AddValue("Test", EncryptionService.Encrypt(Test, Key, IV));
        }

        protected SecureMessenger(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Message = (string)EncryptionService.Decrypt((byte[])info.GetValue("Message", typeof(byte[])), Key, IV);
            MessageID = Convert.ToInt32(EncryptionService.Decrypt((byte[])info.GetValue("MessageID", typeof(byte[])), Key, IV));
            MessagePoints = Convert.ToDouble(EncryptionService.Decrypt((byte[])info.GetValue("MessagePoints", typeof(byte[])), Key, IV));
            Test = EncryptionService.Decrypt((byte[])info.GetValue("Test", typeof(byte[])), Key, IV) as ForTest;
        }

        public void SendMessage()
        {
            Console.WriteLine(Message);
            Console.WriteLine(MessageID);
            Console.WriteLine(MessagePoints);
            Console.WriteLine(FromAncestors);
        }
    }
}
