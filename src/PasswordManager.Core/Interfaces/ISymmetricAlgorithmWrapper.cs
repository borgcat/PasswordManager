using System.Security.Cryptography;

namespace PasswordManager.Core.Interfaces
{
    public interface ISymmetricAlgorithmWrapper
    {
        SymmetricAlgorithm Create { get; }
    }
}