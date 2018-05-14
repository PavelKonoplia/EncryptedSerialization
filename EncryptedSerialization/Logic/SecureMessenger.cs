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
    public class SecureMessenger : EncryptionSerializator
    {

        public string Message { get; set; }

        public SecureMessenger() { }
        public SecureMessenger(IFormatter formatter, string filePath, string message) : base(formatter, filePath)
        {
            Message = message;
        }
        public SecureMessenger(IFormatter formatter, string filePath, FileMode fileMode, string message) : base(formatter, filePath, fileMode)
        {
            Message = message;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Message", EncryptionService.Encrypt(Message, Key, IV));
        }

        protected SecureMessenger(SerializationInfo info, StreamingContext context)
        {
            Message = EncryptionService.Decrypt((byte[])info.GetValue("Message", typeof(byte[])), Key, IV);
        }

        public void SendMessage()
        {
            Console.WriteLine(Message);
        }
    }
}
