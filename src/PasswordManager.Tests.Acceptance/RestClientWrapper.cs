using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace PasswordManager.Tests.Acceptance
{
    public class RestClientWrapper
    {
        private readonly string _uri = ConfigurationManager.AppSettings["WebApiBaseUri"];
        private RestClient _client;

        public RestClientWrapper()
        {
            _client = new RestClient(_uri);
        }

        
        public string GetEncryptedMasterKey(string masterKey)
        {
            var request = new RestRequest("api/KeyManagement", Method.GET);
            request.AddHeader("X-MasterKey", masterKey);
            IRestResponse response = _client.Execute(request);
            
            var content = response.Content;

            return content;
        }

        public string AddToList(string masterKey, PasswordEntity entity)
        {
            var request = new RestRequest("api/passwordManager", Method.POST);
            request.AddHeader("X-Key", masterKey);

            request.AddJsonBody(entity);

            IRestResponse response = _client.Execute(request);

            return response.Content;
        }

        public List<PasswordEntity> GetList(string masterKey)
        {
            var request = new RestRequest("api/passwordManager", Method.GET);
            request.AddHeader("X-Key", masterKey);

            IRestResponse<List<PasswordEntity>> response = _client.Execute<List<PasswordEntity>>(request);

            return response.Data;
        }
    }
}
