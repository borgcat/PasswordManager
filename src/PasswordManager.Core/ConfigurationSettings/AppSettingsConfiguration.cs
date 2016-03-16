namespace PasswordManager.Core.ConfigurationSettings
{
    public class AppSettingsConfiguration
    {
        public string StorageLocation { get; set; }
        public string FileName { get; set; }
        public string EncryptionSalt { get; set; }
        public string EnvironmentName { get; set; }
        public string ProjectVersion { get; set; }
    }
}