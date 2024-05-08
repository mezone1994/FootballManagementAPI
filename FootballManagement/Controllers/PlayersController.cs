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
    public class PlayersController : ControllerBase
    {
        private readonly FootballDbContext _context;

        public PlayersController(FootballDbContext context)
        {
            _context = context;
        }
        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayers()
        {
            return await _context.Players
                .Include(i => i.Team)
                .Include(i => i.Team.League)
                .Include(i=>i.PlayerTeams)
                .Select(p => new PlayerDTO
                {
                    ID = p.ID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Jersey = p.Jersey,
                    FeePaid = p.FeePaid,
                    EMail = p.EMail,
                    DOB = p.DOB,
                    RowVersion = p.RowVersion,
                    Team = new TeamDTO
                    {
                        Id = p.Team.Id,
                        Name = p.Team.Name,
                        Budget= p.Team.Budget,
                        LeagueCode = p.Team.LeagueCode,
                        League = new LeagueDTO
                        {
                            Name = p.Team.League.Name,
                            Code = p.Team.League.Code
                        }
                    }
                })
                .ToListAsync();
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDTO>> GetPlayer(int id)
        {
            var player = await _context.Players
                .Include(i => i.Team)
                .Include(i => i.Team.League)
                .Select(p => new PlayerDTO
                {
                    ID = p.ID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Jersey = p.Jersey,
                    FeePaid = p.FeePaid,
                    EMail = p.EMail,
                    DOB = p.DOB,
                    RowVersion = p.RowVersion,
                    TeamID = p.TeamID,
                    Team = new TeamDTO
                    {
                        Id = p.Team.Id,
                        Name = p.Team.Name,
                        Budget= p.Team.Budget,
                        LeagueCode = p.Team.LeagueCode,
                        League = new LeagueDTO
                        {
                            Code = p.Team.League.Code,
                            Name = p.Team.League.Name,
                        }
                    }
                })
                .FirstOrDefaultAsync(p => p.ID == id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        // GET: api/PlayersByTeam
        [HttpGet("ByTeam/{Teamid}")]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayersByTeam(int Teamid)
        {
            var players = await _context.Players
                .Include(e => e.Team)
                .Select(p => new PlayerDTO
                {
                    ID = p.ID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Jersey = p.Jersey,
                    FeePaid = p.FeePaid,
                    EMail = p.EMail,
                    DOB = p.DOB,
                    RowVersion = p.RowVersion,
                    TeamID = p.TeamID,
                    Team = new TeamDTO
                    {
                        Id = p.Team.Id,
                        Name = p.Team.Name,
                        Budget= p.Team.Budget,
                        LeagueCode = p.Team.LeagueCode,
                        League = new LeagueDTO
                        {
                            Code = p.Team.League.Code,
                            Name = p.Team.League.Name,
                        }
                    }
                })
                .Where(e => e.TeamID == Teamid)
                .ToListAsync();

            if (players.Count() > 0)
            {
                return players;
            }
            else
            {
                return NotFound(new { message = "Error: No Player records for that Team." });
            }
        }

        // PUT: api/Players/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, PlayerDTO player)
        {
            if (id != player.ID)
            {
                return BadRequest(new { message = "Error: ID does not match any Player" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the record you want to update
            var playerToUpdate = await _context.Players.FindAsync(id);

            //Check that you got it
            if (playerToUpdate == null)
            {
                return NotFound(new { message = "Error: no Player record not found." });
            }

            //Wow, we have a chance to check for concurrency even before bothering
            //the database!  Of course, it will get checked again in the database just in case
            //it changes after we pulled the record.  
            //Note using SequenceEqual becuase it is an array after all.
            if (player.RowVersion != null)
            {
                if (!playerToUpdate.RowVersion.SequenceEqual(player.RowVersion))
                {
                    return Conflict(new { message = "Concurrency Error: Player has been changed by another user.  Try editing the record again." });
                }
            }
            //Update the properties of the entity object from the DTO object
            playerToUpdate.ID = player.ID;
            playerToUpdate.FirstName = player.FirstName;
            playerToUpdate.LastName = player.LastName;
            playerToUpdate.Jersey = player.Jersey;
            playerToUpdate.DOB = player.DOB;
            playerToUpdate.FeePaid = player.FeePaid;
            playerToUpdate.EMail = player.EMail;
            playerToUpdate.RowVersion = player.RowVersion;
            playerToUpdate.TeamID = player.TeamID;

            //Put the original RowVersion value in the OriginalValues collection for the entity
            _context.Entry(playerToUpdate).Property("RowVersion").OriginalValue = player.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Player has been Updated." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Player has been updated by another user.  Back out and try editing the record again." });
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Email." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // POST: api/Players
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(PlayerDTO playerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Player player = new Player
            {
                ID = playerDTO.ID,
                FirstName = playerDTO.FirstName,
                LastName = playerDTO.LastName,
                Jersey = playerDTO.Jersey,
                DOB = playerDTO.DOB,
                FeePaid = playerDTO.FeePaid,
                EMail = playerDTO.EMail,
                RowVersion = playerDTO.RowVersion,
                TeamID = playerDTO.TeamID
            };

            try
            {
                _context.Players.Add(player);
                await _context.SaveChangesAsync();

                playerDTO.ID = player.ID;
                playerDTO.RowVersion = player.RowVersion;

                return CreatedAtAction(nameof(GetPlayer), new { id = player.ID }, playerDTO);
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Email." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound(new { message = "Delete Error: Player was already removed from the database." });
            }
            try
            {
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete the player." });
            }
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.ID == id);
        }
    }
}
