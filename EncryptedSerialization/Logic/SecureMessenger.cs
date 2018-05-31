using System;
using System.IO;
using System.Runtime.Serialization;

namespace EncryptedSerialization.Logic
{
    [Serializable]
    public class SecureMessenger : EncryptionSerializator
    {
        public string Message { get; set; }

        public int MessageID { get; set; }

        public double MessagePoints { get; set; }
        
        public SecureMessenger() { }

        public SecureMessenger(IFormatter formatter, string filePath,string ancient, string message, int messageID, double messagePoints) : base(formatter, filePath, ancient)
        {
            Message = message;
            MessageID = messageID;
            MessagePoints = messagePoints;
        }
        public SecureMessenger(IFormatter formatter, string filePath, FileMode fileMode, string ancient,string message, int messageID, double messagePoints) : base(formatter, filePath, ancient, fileMode)
        {
            Message = message;
            MessageID = messageID;
            MessagePoints = messagePoints;
        }

        protected SecureMessenger(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public void SendMessage()
        {
            Console.WriteLine(Message);
            Console.WriteLine(MessageID);
            Console.WriteLine(MessagePoints);
            Console.WriteLine(FromAncestors);
        }
    }
}
