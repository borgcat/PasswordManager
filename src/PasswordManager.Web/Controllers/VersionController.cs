using System.Reflection;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PasswordManager.Web.Controllers
{
    [Route("/Version")]
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
                        Web = new
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
