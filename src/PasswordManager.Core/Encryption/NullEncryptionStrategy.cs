using Microsoft.Extensions.Configuration;
using PasswordManager.Core.Interfaces;

namespace PasswordManager.Core.Encryption
{
    public class NullEncryptionStrategy : IEncryptionStrategy
    {
        public string Encrypt(string value, string masterKey)
        {
            return value;
        }

        public string Decrypt(string text, string masterKey)
        {
            return text;
        }

        public IConfigurationSettings Configuration { get; set; }
       
    }
}