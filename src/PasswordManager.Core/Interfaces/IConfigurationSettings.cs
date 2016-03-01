namespace PasswordManager.Core.Interfaces
{
    public interface IConfigurationSettings
    {
        string StorageLocation { get; }
        string EncriptedFileName { get; }
        string EncryptionSalt { get; set; }
        string GetFullPath();

        string MasterKey { set; }
        string DecryptedMasterKey { get; }
        string EncryptedMasterKey { get; set; }
    }
}