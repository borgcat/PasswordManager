using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using PasswordManager.Core.Exceptions;
using PasswordManager.Core.Interfaces;

namespace PasswordManager.Core.Encryption
{
    public class TripleDESEncryptionStrategy : IEncryptionStrategy
    {
        private readonly IConfigurationSettings _configuration;

        public TripleDESEncryptionStrategy(IConfigurationSettings configuration)
        {
            _configuration = configuration;
        }

        public string Encrypt(string value, string masterKey)
        {
            if(_configuration == null)
                throw new EncryptionConfigurationException("Invalid configuration set in TripleDESEncryptionStrategy");

            return CipherUtility.Encrypt<TripleDESAlgorithm>(value, masterKey, _configuration.EncryptionSalt);
        }

        public string Decrypt(string text, string masterKey)
        {
            if (_configuration == null)
                throw new EncryptionConfigurationException("Invalid configuration set in TripleDESEncryptionStrategy");

            try
            {
                return CipherUtility.Decrypt<TripleDESAlgorithm>(text, masterKey, _configuration.EncryptionSalt);
            }
            catch (System.Security.Cryptography.CryptographicException cryptographicException)
            {
                throw new EncryptionStrategyException(cryptographicException.Message);
            }
        }
    }
}
