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
        [ServiceFilter(typeof(MasterKeyHeaderFilter))]
        public PasswordEntity Get(string id)
        {
            return _repository.GetById(id, Cloner);
        }

        // api doesn't need a member cloner
        private PasswordEntity Cloner(PasswordEntity passwordEntity)
        {
            return passwordEntity;
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
        [ServiceFilter(typeof(MasterKeyHeaderFilter))]
        public PasswordEntity Put(string id, [FromBody]PasswordEntity entity)
        {
            return _repository.Update(id, passwordEntity =>
            {
                passwordEntity.CommonName = entity.Password;
                passwordEntity.Password = entity.Password;
                passwordEntity.Url = entity.Url;
                passwordEntity.UserName = entity.UserName;
            });
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(MasterKeyHeaderFilter))]
        public string Delete(string id)
        {
            if (_repository.Delete(id))
                return "Success";

            return "Failure";
        }
    }
}
