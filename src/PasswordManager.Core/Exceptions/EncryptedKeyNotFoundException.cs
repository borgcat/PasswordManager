using System;

namespace PasswordManager.Core.Exceptions
{
    public class EncryptedKeyNotFoundException : Exception
    {
        public EncryptedKeyNotFoundException(string encryptedkeyNotFoundInKeyStoreHasYourSessionExpired) : base(encryptedkeyNotFoundInKeyStoreHasYourSessionExpired) { }
    }
}