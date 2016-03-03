using System;

namespace PasswordManager.Core.Exceptions
{
    public class EncryptionConfigurationException : Exception
    {
        public EncryptionConfigurationException(string message) : base(message) {}
    }
}