using LocalRNC.Data;
using LocalRNC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LocalRNC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<CompanyController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> Get()
        {
            return await _context.companies.ToListAsync();
        }

        // GET api/<CompanyController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> Get(int id)
        {
            var company = await _context.companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();  // Returns 404 if the company is not found
            }

            return Ok(company);
        }

        // POST api/<CompanyController>
        [HttpPost]
        public async Task<ActionResult<Company>> Post([FromBody] Company company)
        {
            if (company == null)
            {
                return BadRequest("Company data is null");
            }

            // Add the new company to the context
            _context.companies.Add(company);
            await _context.SaveChangesAsync();  // Save changes to the database

            return CreatedAtAction(nameof(Get), new { id = company.Id }, company);
        }

        // PUT api/<CompanyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<CompanyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
