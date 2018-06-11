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
        
        protected EncryptionSerializator()
        {
            GetServiceFromAttribute();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Type type = GetType();
            PropertyInfo[] propsInfos = type.GetProperties();
            FieldInfo[] fieldsInfos = type.GetFields();

            if (EncryptionService != null)
            {
                foreach (PropertyInfo prop in propsInfos)
                {
                    info.AddValue(prop.Name, EncryptionService.Encrypt(prop.GetValue(this)));
                }
                foreach (FieldInfo field in fieldsInfos)
                {
                    info.AddValue(field.Name, EncryptionService.Encrypt(field.GetValue(this)));
                }
            }
            else
            {
                foreach (PropertyInfo prop in propsInfos)
                {
                    info.AddValue(prop.Name, prop.GetValue(this));
                }
                foreach (FieldInfo field in fieldsInfos)
                {
                    info.AddValue(field.Name, field.GetValue(this));
                }
            }
        }

        protected EncryptionSerializator(SerializationInfo info, StreamingContext context)
        {
            GetServiceFromAttribute();

            Type type = GetType();
            PropertyInfo[] propsInfos = type.GetProperties();
            FieldInfo[] fieldsInfos = type.GetFields();

            if (EncryptionService != null)
            {
                foreach (PropertyInfo prop in propsInfos)
                {
                    prop.SetValue(this, Convert.ChangeType(EncryptionService.Decrypt((byte[])info.GetValue(prop.Name, typeof(byte[]))), prop.PropertyType));
                }
                foreach (FieldInfo field in fieldsInfos)
                {
                    field.SetValue(this, Convert.ChangeType(EncryptionService.Decrypt((byte[])info.GetValue(field.Name, typeof(byte[]))), field.FieldType));
                }
            }
            else
            {
                foreach (PropertyInfo prop in propsInfos)
                {
                    prop.SetValue(this, info.GetValue(prop.Name, prop.PropertyType));
                }
                foreach (FieldInfo field in fieldsInfos)
                {
                    field.SetValue(this, info.GetValue(field.Name, field.FieldType));
                }
            }
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
