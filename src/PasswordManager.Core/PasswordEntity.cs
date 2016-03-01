using PasswordManager.Core.Interfaces;

namespace FME.PasswordManager
{
    public class PasswordEntity : IEntity
    {
        public string Id { get; set; }
        public string CommonName { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}