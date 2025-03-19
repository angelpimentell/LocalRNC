using LocalRNC.Data;
using LocalRNC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LocalRNC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<ActionResult<Company>> Put(int id, [FromBody] Company company)
        {
            var existingCompany = await _context.companies.FindAsync(id);

            if (existingCompany == null)
            {
                return NotFound();
            }

            _context.Entry(company).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();

        }

        // DELETE api/<CompanyController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Company>> Delete(int id)
        {
            var company = await _context.companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            _context.companies.Remove(company);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
