using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Tests.Acceptance
{
    public class PasswordEntity
    {
        public string Id { get; set; }
        public string CommonName { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
