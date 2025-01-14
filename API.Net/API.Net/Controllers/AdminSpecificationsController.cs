using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Net.Models;
using API.net.ViewModel;

namespace API.Net.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminSpecificationsController : ControllerBase
    {
        private readonly DbtestContext _context;

        public AdminSpecificationsController(DbtestContext context)
        {
            _context = context;
        }

        // GET: api/Specifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Specification>>> GetSpecifications()
        {
            return Ok(await _context.Specifications.ToListAsync());
        }

        // GET: api/Specifications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Specification>> GetSpecification(string id)
        {
            var specification = await _context.Specifications.FindAsync(id);

            if (specification == null)
            {
                return NotFound();
            }

            return Ok(specification);
        }

        // PUT: api/Specifications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutSpecification([FromBody] UpdateSpecModel model)
        {
            if (model.SpecificationId == null)
            {
                return BadRequest();
            }

            var spec = new Specification()
            {
                SpecificationId = model.SpecificationId,
                Dimensions = model.Dimensions,
                Material = model.Material,
                Original = model.Original,
                Standard = model.Standard
            };

            _context.Specifications.Attach(spec);

            _context.Entry(spec).Property(s => s.Dimensions).IsModified = true;
            _context.Entry(spec).Property(s => s.Standard).IsModified = true;
            _context.Entry(spec).Property(s => s.Material).IsModified = true;
            _context.Entry(spec).Property(s => s.Original).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecificationExists(model.SpecificationId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Specifications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Specification>> PostSpecification(Specification specification)
        {
            _context.Specifications.Add(specification);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SpecificationExists(specification.SpecificationId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSpecification", new { id = specification.SpecificationId }, specification);
        }

        // DELETE: api/Specifications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecification(string id)
        {
            var specification = await _context.Specifications.FindAsync(id);
            if (specification == null)
            {
                return NotFound();
            }

            _context.Specifications.Remove(specification);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpecificationExists(string id)
        {
            return _context.Specifications.Any(e => e.SpecificationId == id);
        }
    }
}
