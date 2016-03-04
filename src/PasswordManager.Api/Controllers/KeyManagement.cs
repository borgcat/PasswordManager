using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PasswordManager.Core;
using PasswordManager.Core.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PasswordManager.Api.Controllers
{
    [Route("api/[controller]")]
    public class KeyManagement : Controller
    {
        private readonly IConfigurationSettings _settings;
        private readonly IRepository<PasswordEntity> _repository;

        public KeyManagement(IConfigurationSettings settings, IRepository<PasswordEntity> repository)
        {
            _settings = settings;
            _repository = repository;
        }

        [HttpGet]
        [ServiceFilter(typeof(EnsureMasterKeyHeaderFilter))]
        public string Get()
        {
            var masterKey = HttpContext.Request.GetFirstHeaderValueOrDefault<string>("X-MasterKey");
            if (!String.IsNullOrWhiteSpace(masterKey))
            {
                _settings.MasterKey = masterKey;

                _repository.EnsureRepository();

                return _settings.EncryptedMasterKey;
            }

            throw new ApiParameterNullException("MasterKey was not been provided via http header");
        }
    }
}
