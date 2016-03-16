using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;

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

            return Content(String.Format("PasswordManager.Api Version: {0}", projectVersion));
        }
    }
}
