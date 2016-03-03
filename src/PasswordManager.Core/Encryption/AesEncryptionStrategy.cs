using System;
using PasswordManager.Core.Exceptions;
using PasswordManager.Core.Interfaces;


namespace PasswordManager.Core.Encryption
{
    public class AesEncryptionStrategy : IEncryptionStrategy
    {
        public AesEncryptionStrategy(IConfigurationSettings configuration)
        {
            Configuration = configuration;
        }

        public string Encrypt(string value, string masterKey)
        {
            if (Configuration == null)
                throw new EncryptionConfigurationException("Invalid configuration set in TripleDESEncryptionStrategy");

            return CipherUtility.Encrypt<AesAlgorithm>(value, masterKey, Configuration.EncryptionSalt);
        }

        public string Decrypt(string text, string masterKey)
        {
            if (Configuration == null)
                throw new EncryptionConfigurationException("Invalid configuration set in TripleDESEncryptionStrategy");

            try
            {
                return CipherUtility.Decrypt<AesAlgorithm>(text, masterKey, Configuration.EncryptionSalt);
            }
            catch (System.Security.Cryptography.CryptographicException cryptographicException)
            {
                throw new EncryptionStrategyException(cryptographicException.Message);
            }
        }

        public IConfigurationSettings Configuration { get; set; }
    }
}