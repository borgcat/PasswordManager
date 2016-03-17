using System;
using System.Collections.Generic;
using Shouldly;
using TechTalk.SpecFlow;

namespace PasswordManager.Tests.Acceptance
{
    [Binding]
    public class PasswordManager_AddPasswordSteps
    {
        private RestClientWrapper Client { get; } = new RestClientWrapper();

        [Given(@"Given I have a secret of '(.*)'")]
        public void GivenGivenIHaveASecretOf(string p0)
        {
            ScenarioContext.Current.Add("encryptedKey", Client.GetEncryptedMasterKey(p0));
        }
        
        [Given(@"I give my password and site details")]
        public void GivenIGiveMyPasswordAndSiteDetails()
        {
            string key = ScenarioContext.Current.Get<string>("encryptedKey");
            ScenarioContext.Current.Add("AddedEntityId", Client.AddToList(key, new PasswordEntity
            {
                  CommonName = "CommonNamestring",
                  Url = "Urlstring",
                  UserName = "UserNamestring",
                  Password = "Passwordstring"
            }));
        }
        
        [Then(@"the result should be count of passwords greater than zero after I add one")]
        public void ThenTheResultShouldBeCountOfPasswordsGreaterThanZeroAfterIAddOne()
        {
            List<PasswordEntity> list = Client.GetList(ScenarioContext.Current.Get<string>("encryptedKey"));
            list.Count.ShouldBeGreaterThan(0);
        }
    }
}
