using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using PasswordManager.Core.Exceptions;
using PasswordManager.Core.Interfaces;
using Serilog;

namespace PasswordManager.Core.Serialization
{
    public class FileSerialization<T> : ISerialization<T>
    {
        private readonly IConfigurationSettings _configuration;
        private readonly IEncryptionStrategy _encryptionStrategy;
        private readonly ILogger _logger;

        public FileSerialization(IConfigurationSettings configuration, IEncryptionStrategy encryptionStrategy, ILogger logger)
        {
            _configuration = configuration;
            _encryptionStrategy = encryptionStrategy;
            _logger = logger;
        }

        private static void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (!String.IsNullOrEmpty(directoryName) && (!Directory.Exists(directoryName)))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        public bool EnsureContainer()
        {
            try
            {
                if (File.Exists(_configuration.GetFullPath()) == true)
                {
                    return true;
                }
                else
                {
                    return SerializeObject(new List<T>());
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return false;
        }

        public bool SerializeObject(object o)
        {
            try
            {
                EnsureFolder(_configuration.StorageLocation);

                string json = JsonConvert.SerializeObject(o);
                string encrypted = _encryptionStrategy.Encrypt(json, _configuration.DecryptedMasterKey);

                File.WriteAllText(_configuration.GetFullPath(), encrypted);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return false;
        }

        public List<T> DeserializeObject()
        {
            try
            {
                EnsureFolder(_configuration.StorageLocation);

                if (File.Exists(_configuration.GetFullPath()) == false)
                {
                    return new List<T>();
                }

                using (StreamReader file = File.OpenText(_configuration.GetFullPath()))
                {
                    string encryptedFile = file.ReadToEnd();

                    string decriptedText = _encryptionStrategy.Decrypt(encryptedFile, _configuration.DecryptedMasterKey);

                    return JsonConvert.DeserializeObject<List<T>>(decriptedText);
                }
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                _logger.Error(ex.Message);
                throw new EncryptedDeserializationException(ex.Message);
            }
        }

    }
}
