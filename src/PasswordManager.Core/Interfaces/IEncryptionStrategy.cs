namespace PasswordManager.Core.Interfaces
{
    public interface IEncryptionStrategy
    {
        string Encrypt(string value, string masterKey);
        string Decrypt(string text, string masterKey);
    }
}
