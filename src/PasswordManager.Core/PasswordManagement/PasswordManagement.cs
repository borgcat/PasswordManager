using System;
using PasswordManager.Core.Interfaces;

namespace PasswordManager.Core.PasswordManagement
{
    public class PasswordManagement : IPasswordManagement
    {
        public string GeneratePassword()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
