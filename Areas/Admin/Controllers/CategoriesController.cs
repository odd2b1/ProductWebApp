using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NToastNotify;
using ProductWebApp.Areas.Admin.ViewModels;
using ProductWebApp.Models;
using ProductWebApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProductWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IToastNotification _toastNotification;
        private readonly Uri _baseUrl;

        public CategoriesController(IConfiguration configuration, IToastNotification toastNotification)
        {
            _configuration = configuration;
            _toastNotification = toastNotification;
            _baseUrl = new Uri(_configuration["BaseUrl"]);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = _baseUrl;
                httpclient.DefaultRequestHeaders.Authorization = getToken();
                using (var result = await httpclient.GetAsync("Categories"))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonstring = await result.Content.ReadAsStringAsync();
                        var content = JsonConvert.DeserializeObject<List<Category>>(jsonstring);
                        return View(content.OrderByDescending(x=>x.DateStamp).ToList());
                    }
                    return View("Error");
                }
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoriesAddVM model)
        {
            if (ModelState.IsValid)
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = _baseUrl;
                    httpclient.DefaultRequestHeaders.Authorization = getToken();
                    var jsonobj = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonobj, Encoding.UTF8, "application/json");
                    using (var result = await httpclient.PostAsync("Categories", content))
                    {
                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        ModelState.AddModelError("", "Something Went Wrong");
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = _baseUrl;
                httpclient.DefaultRequestHeaders.Authorization = getToken();
                using (var result = await httpclient.GetAsync($"Categories/{id}"))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        var stringcon = await result.Content.ReadAsStringAsync();
                        var jsonobj = JsonConvert.DeserializeObject<Category>(stringcon);
                        var obj = new CategoriesEditVM()
                        {
                            Name = jsonobj.Name,
                            Id = jsonobj.Id
                        };
                        return View(obj);
                    }
                    return View("NotFound", ViewBag.Error = $"Category with Id-{id} not found.");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoriesEditVM model)
        {
            if (ModelState.IsValid)
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = _baseUrl;
                    httpclient.DefaultRequestHeaders.Authorization = getToken();
                    var jsoncon = JsonConvert.SerializeObject(model);
                    var stringcon = new StringContent(jsoncon, Encoding.UTF8, "application/json");
                    using (var result = await httpclient.PutAsync("Categories", stringcon))
                    {
                        if (result.IsSuccessStatusCode)
                        {
                            _toastNotification.AddSuccessToastMessage("Updated Successfully");
                        }
                        else
                        {
                            _toastNotification.AddErrorToastMessage("Error Updating");
                        }
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = _baseUrl;
                httpclient.DefaultRequestHeaders.Authorization = getToken();
                var jsonobj = JsonConvert.SerializeObject(id);
                var stringcon = new StringContent(jsonobj, Encoding.UTF8, "application/json");
                using (var result = await httpclient.DeleteAsync($"Categories/{id}"))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        _toastNotification.AddSuccessToastMessage("Delete Successfully");
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage("Error Deleting");
                    }
                    return RedirectToAction("Index", "Categories");
                }
            }
        }

        private AuthenticationHeaderValue getToken()
        {
            var token = Request.Cookies["Auth"];
            var result = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return result;
        }
    }
}
