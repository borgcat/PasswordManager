using System.Security.Cryptography;
using PasswordManager.Core.Interfaces;

namespace PasswordManager.Core.Encryption
{
    public class AesAlgorithm : ISymmetricAlgorithmWrapper
    {
        public SymmetricAlgorithm Create => Aes.Create();
    }

    public class TripleDESAlgorithm : ISymmetricAlgorithmWrapper
    {
        public SymmetricAlgorithm Create => TripleDES.Create();
    }
}