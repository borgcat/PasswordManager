namespace PasswordManager.Core.Interfaces
{
    public interface IEncryptionStrategy
    {
        string Encrypt(string value);
        string Decrypt(string text);
    }
}
