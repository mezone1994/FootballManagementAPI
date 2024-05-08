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
    public class TeamsController : ControllerBase
    {
        private readonly FootballDbContext _context;

        public TeamsController(FootballDbContext context)
        {
            _context = context;
        }
        // GET: api/Teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeams()
        {
            return await _context.Teams
                .Select(p => new TeamDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Budget = p.Budget,
                    LeagueCode = p.LeagueCode,
                })
                .ToListAsync();
        }
        // GET: api/Teams/inc
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeamsInc()
        {
            return await _context.Teams
                .Include(d => d.Players)
                .Include(d => d.League)
                .Select(d => new TeamDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Budget = d.Budget,
                    LeagueCode = d.LeagueCode,
                    Players = d.Players.Select(dplayer => new PlayerDTO
                    {
                        ID = dplayer.ID,
                        FirstName = dplayer.FirstName,
                        LastName = dplayer.LastName,
                        Jersey = dplayer.Jersey,
                        DOB = dplayer.DOB,
                        FeePaid = dplayer.FeePaid,
                        EMail = dplayer.EMail,
                        TeamID = dplayer.TeamID
                    }).ToList()
                })
                .ToListAsync();
        }
        // GET: api/teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDTO>> GetTeam(int id)
        {
            var teamDTO = await _context.Teams
                .Include(d => d.Players)
                .Include(d => d.League)
                .Select(d => new TeamDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Budget = d.Budget,
                    LeagueCode = d.LeagueCode,
                    Players = d.Players.Select(dplayer => new PlayerDTO
                    {
                        ID = dplayer.ID,
                        FirstName = dplayer.FirstName,
                        LastName = dplayer.LastName,
                        Jersey = dplayer.Jersey,
                        DOB = dplayer.DOB,
                        FeePaid = dplayer.FeePaid,
                        EMail = dplayer.EMail,
                        TeamID = dplayer.TeamID
                    }).ToList()
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (teamDTO == null)
            {
                return NotFound(new { message = "Error: Team not found." });
            }

            return teamDTO;
        }

        // GET: api/teams/league/{LeagueCode}
        [HttpGet("byLeague/{leagueCode}")]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeamsByLeague(string leagueCode)
        {

            return await _context.Teams
               .Include(d => d.Players)
               .Include(d => d.League)
               .Select(d => new TeamDTO
               {
                   Id = d.Id,
                   Name = d.Name,
                   Budget = d.Budget,
                   LeagueCode = d.LeagueCode,
                   Players = d.Players.Select(dplayer => new PlayerDTO
                   {
                       ID = dplayer.ID,
                       FirstName = dplayer.FirstName,
                       LastName = dplayer.LastName,
                       Jersey = dplayer.Jersey,
                       DOB = dplayer.DOB,
                       FeePaid = dplayer.FeePaid,
                       EMail = dplayer.EMail,
                       TeamID = dplayer.TeamID
                   }).ToList()

               })
               .Where(e => e.LeagueCode == leagueCode).ToListAsync();
        }

        // PUT: api/teams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, Team team)
        {
            if (id != team.Id)
            {
                return BadRequest();
            }

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
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

        // POST: api/Teams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Team>> PostTeam(TeamDTO teamDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Team team = new Team
            {
                //Update the properties of the entity object from the DTO object
                Name = teamDTO.Name,
                Budget = teamDTO.Budget,
                LeagueCode = teamDTO.LeagueCode,
            };

            try
            {
                _context.Teams.Add(team);
                await _context.SaveChangesAsync();
                //Assign Database Generated values back into the DTO
                teamDTO.Id = team.Id;
                return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, teamDTO);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
            }
        }

        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound(new { message = "Delete Error: Team has already been removed." });
            }
            try
            {
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    return BadRequest(new { message = "Delete Error: Remember, you cannot delete a Team that has Players assigned." });
                }
                else
                {
                    return BadRequest(new { message = "Delete Error: Unable to delete Team. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
