using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PasswordManager.Core;


namespace PasswordManager.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       
        [HttpGet]
        public async Task<JsonResult>  GetListOfPassword()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:38183");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-MasterKey", "Test1234");

               // New code:
               HttpResponseMessage response = await client.GetAsync("api/PasswordManager");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    //List<PasswordEntity> passwords = await response.Content.ReadAsAsync<List<PasswordEntity>>();

                    return new JsonResult(apiResponse);
                }
            }

            return null;
        }
    }
}
