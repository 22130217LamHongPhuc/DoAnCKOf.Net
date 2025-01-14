using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Net.Models;

namespace API.Net.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminSubcategoriesController : ControllerBase
    {
        private readonly DbtestContext _context;

        public AdminSubcategoriesController(DbtestContext context)
        {
            _context = context;
        }

        // GET: api/AdminSubcategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subcategory>>> GetSubcategories()
        {
            return Ok(await _context.Subcategories.ToListAsync());
        }

        // GET: api/AdminSubcategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subcategory>> GetSubcategory(string id)
        {
            var subcategory = await _context.Subcategories.FindAsync(id);

            if (subcategory == null)
            {
                return NotFound();
            }

            return subcategory;
        }

        // PUT: api/AdminSubcategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubcategory(string id, Subcategory subcategory)
        {
            if (id != subcategory.SubcategoryId)
            {
                return BadRequest();
            }

            _context.Entry(subcategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubcategoryExists(id))
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

        // POST: api/AdminSubcategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subcategory>> PostSubcategory(Subcategory subcategory)
        {
            _context.Subcategories.Add(subcategory);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SubcategoryExists(subcategory.SubcategoryId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSubcategory", new { id = subcategory.SubcategoryId }, subcategory);
        }

        // DELETE: api/AdminSubcategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubcategory(string id)
        {
            var subcategory = await _context.Subcategories.FindAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }

            _context.Subcategories.Remove(subcategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubcategoryExists(string id)
        {
            return _context.Subcategories.Any(e => e.SubcategoryId == id);
        }
    }
}
