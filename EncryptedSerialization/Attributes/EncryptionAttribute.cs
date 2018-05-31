using System;
using System.Linq;
using System.Reflection;

namespace EncryptedSerialization.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class EncryptionAttribute : Attribute
    {
        public IEncryptionService EncryptionService { get; set; }

        public EncryptionAttribute(string serviceName, params int[] parameters)
        {
            SetService(serviceName, parameters);
        }

        private void SetService(string serviceName, params int[] parameters)
        {
            try
            {
                Type service = Assembly.GetExecutingAssembly().GetType(serviceName);
                ConstructorInfo[] constructorInfo = service.GetConstructors();
                EncryptionService = constructorInfo[0].Invoke(parameters.Cast<object>().ToArray()) as IEncryptionService;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
