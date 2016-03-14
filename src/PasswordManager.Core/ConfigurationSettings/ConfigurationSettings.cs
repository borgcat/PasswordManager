using System;
using System.Text;
using Microsoft.Extensions.OptionsModel;
using PasswordManager.Core.Interfaces;

namespace PasswordManager.Core.ConfigurationSettings
{
    public class ConfigurationSettings : IConfigurationSettings
    {
        private readonly IKeyPersistenceStrategy _keyPersistenceStrategy;
        private readonly IOptions<AppSettingsConfiguration> _appSettings;

        private string _encryptedMasterKey;
        public ConfigurationSettings(IKeyPersistenceStrategy keyPersistenceStrategy, IOptions<AppSettingsConfiguration> appSettings)
        {
            _keyPersistenceStrategy = keyPersistenceStrategy;
            _appSettings = appSettings;

            if(!String.IsNullOrEmpty(_appSettings.Value.StorageLocation))
                StorageLocation = _appSettings.Value.StorageLocation;
        }

        public string StorageLocation { get; set; }

        public string EncryptionSalt
        {
            get { return _appSettings.Value.EncryptionSalt; }
            set { _appSettings.Value.EncryptionSalt = value; }
        }

        public string FileExtension => EnvironmentName;
        public string EnvironmentName => String.IsNullOrWhiteSpace(_appSettings.Value.EnvironmentName) ? "Production" : _appSettings.Value.EnvironmentName;
        
        public string EncriptedFileName 
        {
            get
            {
                var encryptedFilename = EncryptedMasterKey;
                var encodedFileName = Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptedFilename));
                var encodedAndEscapedFilename = encodedFileName.Replace('/', '-');

                return $"{encodedAndEscapedFilename}.{FileExtension}";
            }
        }

        public string DecryptedMasterKey => _keyPersistenceStrategy.DecryptedMasterKey(MasterKey);

        public string EncryptedMasterKey
        {
            get
            {
                return _keyPersistenceStrategy.EncryptedMasterKey(MasterKey);
            }

            set { _keyPersistenceStrategy.EncryptedMasterKey(value); }
        } 

        // shared resource
        public string MasterKey
        {
            set { _encryptedMasterKey = _keyPersistenceStrategy.AddOrRetrieveMasterKey(value); }
            get { return _encryptedMasterKey;}
        }

        public string GetFullPath()
        {
            if (string.IsNullOrWhiteSpace(StorageLocation) || string.IsNullOrWhiteSpace(EncriptedFileName))
                throw new Exception("Path appsettings are missing");

            return $"{StorageLocation}\\{EncriptedFileName}";
        }

    }
}
