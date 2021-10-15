using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProductWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProductWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly Uri _baseUrl;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
            _baseUrl = new Uri(_configuration["BaseUrl"]);
            
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = _baseUrl;
                    var jsonobj = JsonConvert.SerializeObject(model);
                    var stringcon = new StringContent(jsonobj, Encoding.UTF8, "application/json");
                    using (var result = await httpclient.PostAsync("Account/Login", stringcon))
                    {
                        if (!result.IsSuccessStatusCode)
                        {
                            ModelState.AddModelError("", "Invalid Credentials");
                            return View();
                        }
                        var token = await result.Content.ReadAsStringAsync();
                        //HttpContext.Session.SetString("JWT", token);
                        Response.Cookies.Append("Auth", token, new CookieOptions { Expires = DateTime.Now.AddDays(1)});
                    }
                }
                return RedirectToAction("Index", "Home", new { Areas = "" });
            }
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("Auth");
            return RedirectToAction("Index", "Home", new { Areas = "" });
        }
    }
}
