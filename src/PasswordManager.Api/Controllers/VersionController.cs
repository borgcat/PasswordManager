using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PasswordManager.Api.Controllers
{
    [Route("/")]
    public class VersionController : Controller
    {
       // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            string projectVersion = GetType()
            .GetTypeInfo()
            .Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;

            var json = new
            {
                PasswordManager = new
                {
                    Project = new
                    {
                        Api = new
                        {
                            Version = projectVersion
                        }
                    }
                }
            };

            return Content(JsonConvert.SerializeObject(json));
        }
    }
}
