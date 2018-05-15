using EncryptedSerialization.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedSerialization.Logic
{

    [Serializable]
    [Encryption("EncryptedSerialization.Service.EncryptionService", 1111, 1111)]
    public abstract class EncryptionSerializator : ISerializable
    {
        protected IFormatter SerializationFormatter { get; set; }
        protected string FilePath { get; set; }
        protected FileMode SerializationFileMode { get; set; }
        protected IEncryptionService EncryptionService { get; set; }

        protected string FromAncestors { get; set; }

        protected byte[] Key { get; set; }
        protected byte[] IV { get; set; }
        protected EncryptionSerializator()
        {
            GetServiceFromAttribute();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FromAncestors", EncryptionService.Encrypt(FromAncestors, Key, IV));
        }

        protected EncryptionSerializator(SerializationInfo info, StreamingContext context)
        {
            GetServiceFromAttribute();
            FromAncestors = (string)(EncryptionService.Decrypt((byte[])info.GetValue("FromAncestors", typeof(byte[])), Key, IV));
        }

        public EncryptionSerializator(IFormatter formatter, string filePath, string acient, FileMode fileMode = FileMode.OpenOrCreate)
        {
            SerializationFormatter = formatter;
            FilePath = filePath;
            SerializationFileMode = fileMode;
            FromAncestors = acient;
            GetServiceFromAttribute();
        }

        protected void GetServiceFromAttribute()
        {
            var type = GetType();
            if (Attribute.IsDefined(type, typeof(EncryptionAttribute)))
            {
                var encryptionAttribute = Attribute.GetCustomAttribute(type, typeof(EncryptionAttribute)) as EncryptionAttribute;
                EncryptionService = encryptionAttribute.EncryptionService;
                Key = encryptionAttribute.Key;
                IV = encryptionAttribute.IV;
            }
        }

        public virtual void Serialization()
        {
            using (FileStream fs = new FileStream(FilePath, SerializationFileMode))
            {
                SerializationFormatter.Serialize(fs, this);
                Console.WriteLine("Object serialized");
            }
        }

        public virtual object Deserialization(SecureMessenger o)
        {
            using (FileStream fs = new FileStream(FilePath, SerializationFileMode))
            {
                o = (SecureMessenger)SerializationFormatter.Deserialize(fs);
                Console.WriteLine("Object deserialized");
            }
            return o;
        }
    }
}
