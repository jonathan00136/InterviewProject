using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebAPI.Models;

namespace SampleWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerritoriesController : ControllerBase
    {
        private readonly masterContext _context;

        public TerritoriesController(masterContext context)
        {
            _context = context;
        }

        [HttpGet(Name = nameof(GetAllTerritorie))]
        public async Task<ActionResult<IEnumerable<Territories>>> GetAllTerritorie()
        {
            return await _context.Territories.ToListAsync();
        }

        [HttpGet("{id}", Name = nameof(GetTerritorieByIdAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Territories>> GetTerritorieByIdAsync(string id)
        {
            var territorie = await _context.Territories.FindAsync(id);

            if (territorie == null)
            {
                return NotFound();
            }

            return territorie;
        }

        [HttpPut("{id}", Name = nameof(PutTerritorie))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutTerritorie(string id, Territories territorie)
        {
            if (id != territorie.TerritoryId)
            {
                return BadRequest();
            }

            _context.Entry(territorie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TerritorieExists(id))
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Territories>> PostTerritorie(Territories territorie)
        {
            _context.Territories.Add(territorie);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetTerritoriesByIdAsync", new { id = territorie.TerritoryId }, territorie);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteTerritorie(string id)
        {
            var territorie = await _context.Territories.FindAsync(id);
            if (territorie == null)
            {
                return NotFound();
            }

            _context.Territories.Remove(territorie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool TerritorieExists(string id)
        {
            return _context.Territories.Any(e => e.TerritoryId == id);
        }
    }
}
