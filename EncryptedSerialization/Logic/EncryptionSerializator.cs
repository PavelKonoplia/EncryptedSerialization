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

        protected byte[] Key { get; set; }
        protected byte[] IV { get; set; }
        protected EncryptionSerializator()
        {
            GetServiceFromAttribute();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        protected EncryptionSerializator(SerializationInfo info, StreamingContext context)
        {

        }

        public EncryptionSerializator(IFormatter formatter, string filePath, FileMode fileMode = FileMode.OpenOrCreate)
        {
            SerializationFormatter = formatter;
            FilePath = filePath;
            SerializationFileMode = fileMode;
            GetServiceFromAttribute();
        }

        protected void GetServiceFromAttribute()
        {
            var type = GetType();
            if (Attribute.IsDefined(type, typeof(EncryptionAttribute)))
            {
                var attribute = Attribute.GetCustomAttribute(type, typeof(EncryptionAttribute)) as EncryptionAttribute;
                EncryptionService = attribute.EncryptionService;
                Key = attribute.Key;
                IV = attribute.IV;
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
