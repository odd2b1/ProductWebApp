using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebApp.Models;
using Sieve.Models;
using Sieve.Services;

namespace ProductWebApp.Areas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestModelsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SieveProcessor sieveProcessor;

        public TestModelsController(AppDbContext context, SieveProcessor sieveProcessor)
        {
            _context = context;
            this.sieveProcessor = sieveProcessor;
        }

        // GET: api/TestModels
        [HttpGet]
        public async Task<IActionResult> GetTestModels([FromQuery]SieveModel sieveModel)
        {
            var test = _context.TestModels.AsNoTracking();
            test = sieveProcessor.Apply(sieveModel, test);
            var x=  await test.ToListAsync();
            return Ok(x);
        }

        // GET: api/TestModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestModel>> GetTestModel(int id)
        {
            var testModel = await _context.TestModels.FindAsync(id);

            if (testModel == null)
            {
                return NotFound();
            }

            return testModel;
        }

        // PUT: api/TestModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestModel(int id, TestModel testModel)
        {
            if (id != testModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(testModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TestModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TestModel>> PostTestModel(TestModel testModel)
        {
            _context.TestModels.Add(testModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTestModel", new { id = testModel.Id }, testModel);
        }

        // DELETE: api/TestModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestModel(int id)
        {
            var testModel = await _context.TestModels.FindAsync(id);
            if (testModel == null)
            {
                return NotFound();
            }

            _context.TestModels.Remove(testModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TestModelExists(int id)
        {
            return _context.TestModels.Any(e => e.Id == id);
        }
    }
}
