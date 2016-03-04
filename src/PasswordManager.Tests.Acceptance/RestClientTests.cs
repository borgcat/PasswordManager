using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PasswordManager.Tests.Acceptance
{
    [TestFixture]
    public class RestClientTests
    {
        [Test]
        public void RestClientTest()
        {
            RestClientWrapper wrapper = new RestClientWrapper();

            string encryptedKey = wrapper.GetEncryptedMasterKey("Test1234");

            var list = wrapper.GetList(encryptedKey);
        }
    }
}
