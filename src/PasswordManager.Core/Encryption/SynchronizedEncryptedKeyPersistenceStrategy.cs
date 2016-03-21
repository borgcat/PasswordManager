using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using PasswordManager.Core.Exceptions;
using PasswordManager.Core.Interfaces;

namespace PasswordManager.Core.Encryption
{
    public class SynchronizedEncryptedKeyPersistenceStrategy : IKeyPersistenceStrategy
    {
        private readonly Lazy<IConfigurationSettings> _configuration;
        private readonly Lazy<IEncryptionStrategy> _encryptionStrategy;
        private readonly ConcurrentDictionary<string, KeyState> _concurrentState = new ConcurrentDictionary<string, KeyState>();

        public SynchronizedEncryptedKeyPersistenceStrategy(Lazy<IConfigurationSettings> configuration, Lazy<IEncryptionStrategy> encryptionStrategy)
        {
            _configuration = configuration;
            _encryptionStrategy = encryptionStrategy;
        }


        public string AddOrRetrieveMasterKey(string masterKey)
        {
            var encryptedKey = _encryptionStrategy.Value.Encrypt(masterKey, masterKey);

            KeyState outKeyState;
            if (_concurrentState.TryGetValue(encryptedKey, out outKeyState))
            {
                return outKeyState.EnryptedMasterKey;
            }

            var newKeyState = new KeyState()
            {
                DecryptedMasterKey = masterKey,
                EnryptedMasterKey = encryptedKey
            };

            if (_concurrentState.TryAdd(encryptedKey, newKeyState))
            {
                return newKeyState.EnryptedMasterKey;
            }

            return String.Empty;
        }

        public string DecryptedMasterKey(string encryptedMaster)
        {
            KeyState outKeyState;
            if (_concurrentState.TryGetValue(encryptedMaster, out outKeyState))
            {
                return outKeyState.DecryptedMasterKey;
            }

            return null;
        }

        public string EncryptedMasterKey(string encryptedMaster)
        {
            if (string.IsNullOrWhiteSpace(encryptedMaster))
                return null;

            KeyState outKeyState;
            if (_concurrentState.TryGetValue(encryptedMaster, out outKeyState))
            {
                return outKeyState.EnryptedMasterKey;
            }

            throw new EncryptedKeyNotFoundException("EncryptedKey Not Found in Key Store, has your session expired?");
        }

        internal class KeyState
        {
            public string DecryptedMasterKey { get; set; }
            public string EnryptedMasterKey { get; set; }
        }
    }
}