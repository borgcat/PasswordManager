using System;
using System.Collections.Generic;
using Shouldly;
using TechTalk.SpecFlow;

namespace PasswordManager.Tests.Acceptance
{
    [Binding]
    public class PasswordManagerGetPasswordsSteps
    {
        private RestClientWrapper Client { get; } = new RestClientWrapper();

        [Given(@"I have a secret of '(.*)'")]
        public void GivenIHaveASecretOf(string p0)
        {
            ScenarioContext.Current.Add("encryptedKey", Client.GetEncryptedMasterKey(p0));
        }


        [Then(@"the result should be count of passwords greater than zero")]
        public void ThenTheResultShouldBeCountOfPasswordsGreaterThanZero()
        {
            List<PasswordEntity> list = Client.GetList(ScenarioContext.Current.Get<string>("encryptedKey"));
            list.Count.ShouldBeGreaterThan(0);
        }
    }
}
