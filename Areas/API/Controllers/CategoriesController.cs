using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductWebApp.Areas.Admin.ViewModels;
using ProductWebApp.Models;
using ProductWebApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebApp.Areas.API.Controllers
{
    [Area("API")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesRepo _categoriesRepo;

        public CategoriesController(ICategoriesRepo categoriesRepo)
        {
            _categoriesRepo = categoriesRepo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _categoriesRepo.All();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var result = _categoriesRepo.Get(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Post(CategoriesAddVM model)
        {
            if (ModelState.IsValid)
            {
                var obj = new Category()
                {
                    DateStamp = DateTime.Now,
                    Name = model.Name
                };
                var result = _categoriesRepo.Add(obj);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(model);
            }
            return BadRequest();
        }

        [HttpPut]
        public IActionResult Put(CategoriesEditVM model)
        {
            if (ModelState.IsValid)
            {
                var obj = _categoriesRepo.Get(model.Id);
                if (obj == null)
                {
                    return NotFound(new Response { isSuccess = false, Error = $"Category with Id-{model.Id} not Found." });
                }
                obj.Name = model.Name;
                var result = _categoriesRepo.Edit(obj);
                if (result == null)
                {
                    return BadRequest(new Response { isSuccess = false, Error = "Something Went Wrong" });
                }
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id:Guid}")]
        public IActionResult Delete(Guid id)
        {
            var result = _categoriesRepo.Delete(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
