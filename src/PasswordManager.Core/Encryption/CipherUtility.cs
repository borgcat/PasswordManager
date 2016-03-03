﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using PasswordManager.Core.Interfaces;

namespace PasswordManager.Core.Encryption
{
    public class CipherUtility
    {
        public static string Encrypt<T>(string value, string password, string salt) where T : ISymmetricAlgorithmWrapper, new()
        {
            if (String.IsNullOrWhiteSpace(password) || String.IsNullOrWhiteSpace(salt))
                throw new MissingKeyException();

            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            SymmetricAlgorithm algorithm = new T().Create;

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateEncryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream())
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        writer.Write(value);
                    }
                }

                return Convert.ToBase64String(buffer.ToArray());
            }
        }

        public static string Decrypt<T>(string text, string password, string salt) where T : ISymmetricAlgorithmWrapper, new()
        {
            if (String.IsNullOrWhiteSpace(password))
                throw new MissingKeyException();

            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            SymmetricAlgorithm algorithm = new T().Create;

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream(Convert.FromBase64String(text)))
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Unicode))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        // Generate a simple yet strong salt key.  Only alphas in this example, but you can pimp the example easily.
        // Use the RNGCryptoServiceProvider to get random bytes. So should be reasonably random
        //
        // name="maxSize">How much NaCl is required ?
        // returns Random alpha string you can use as SALT
        public static string GenerateSimpleSalt(int maxSize = 64)
        {
            var alphaSet = new char[64]; // use 62 for strict alpha... that random generator for alphas only
                                         //nicer results with set length * int i = 256. But still produces excellent random results.
                                         //alphaset plus 2.  Reduce to 62 if alpha requried
            alphaSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890#!".ToCharArray();
            var tempSB = new StringBuilder(maxSize);

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[maxSize];
                rng.GetBytes(bytes);
               
                foreach (var b in bytes)
                {   // use b , a random from 0-255 as the index to our source array. Just mod on length set
                    tempSB.Append(alphaSet[b % (alphaSet.Length)]);
                }
            }
            return tempSB.ToString();
        }
    }

    public class MissingKeyException : Exception
    {
        public MissingKeyException() : base("The key provided was either null or empty.")
        {
            
        }
    }
}
