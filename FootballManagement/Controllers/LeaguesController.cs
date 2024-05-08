using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballManagement.Data;
using FootballManagement.Models;

namespace FootballManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaguesController : ControllerBase
    {
        private readonly FootballDbContext _context;

        public LeaguesController(FootballDbContext context)
        {
            _context = context;
        }

        // GET: api/Leagues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<League>>> GetLeagues()
        {
            return await _context.Leagues.ToListAsync();
        }

        // GET: api/Teams/inc
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<LeagueDTO>>> GetLeaguesInc()
        {
            var leaguesteams = await _context.Leagues
                .Include(e => e.Teams)
                .Select(p => new LeagueDTO
                {
                    Code = p.Code,
                    Name = p.Name,
                    Teams = p.Teams.Select(t => new TeamDTO
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Budget = t.Budget,
                        LeagueCode = t.LeagueCode

                    }).ToList()
                })
                .ToListAsync();

            return leaguesteams;
        }
        //// GET: api/Leagues/Teams
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<League>>> GetLeaguesTeams()
        //{
        //    return await _context.Leagues
        //        .Include(p => p.Teams).ToListAsync();
        //}

        // GET: api/Leagues/5
        [HttpGet("{code}")]
        public async Task<ActionResult<League>> GetLeague(string code)
        {
            var league = await _context.Leagues.
                Include(l => l.Teams)
                .FirstOrDefaultAsync(l=>l.Code == code);

            if (league == null)
            {
                return NotFound();
            }

            return league;
        }

        // PUT: api/Leagues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{code}")]
        public async Task<IActionResult> PutLeague(string code, League league)
        {
            if (code != league.Code)
            {
                return BadRequest();
            }

            _context.Entry(league).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeagueExists(code))
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

        // POST: api/Leagues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<League>> PostLeague(League league)
        {
            _context.Leagues.Add(league);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LeagueExists(league.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLeague", new { id = league.Code }, league);
        }

        // DELETE: api/Leagues/5
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteLeague(string code)
        {
            var league = await _context.Leagues.FindAsync(code);
            if (league == null)
            {
                return NotFound();
            }

            _context.Leagues.Remove(league);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeagueExists(string code)
        {
            return _context.Leagues.Any(e => e.Code == code);
        }
    }
}
