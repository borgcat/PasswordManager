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
    public class PasswordManagerController : Controller
    {
        private readonly IRepository<PasswordEntity> _repository;

        public PasswordManagerController(IRepository<PasswordEntity> repository)
        {
            _repository = repository;
        }

        // GET: api/values
        [HttpGet]
        [ServiceFilter(typeof(MasterKeyHeaderFilter))]
        public IEnumerable<PasswordEntity> Get()
        {
            return _repository.GetAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [ServiceFilter(typeof(MasterKeyHeaderFilter))]
        public string Post([FromBody]PasswordEntity entity)
        {
            return _repository.Insert(entity);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
