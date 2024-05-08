using FootballManagement.Models;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;

namespace FootballManagement.Data
{
    public class FootballDbContext : DbContext
    {
        //To give access to IHttpContextAccessor for Audit Data with IAuditable
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Property to hold the UserName value
        public string UserName
        {
            get; private set;
        }

        public FootballDbContext(DbContextOptions<FootballDbContext> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            if (_httpContextAccessor.HttpContext != null)
            {
                //We have a HttpContext, but there might not be anyone Authenticated
                UserName = _httpContextAccessor.HttpContext?.User.Identity.Name;
                UserName ??= "Unknown";
            }
            else
            {
                //No HttpContext so seeding data
                UserName = "Seed Data";
            }
        }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerTeam> PlayerTeams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Team>()
            //    .HasOne(pt => pt.League)
            //    .WithMany(t => t.Teams)
            //    .HasForeignKey(pt => pt.LeagueCode)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany(p => p.Players)
                .WithOne(p=> p.Team)
                .OnDelete(DeleteBehavior.Restrict);

            // add unique index for email
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.EMail)
                .IsUnique();

            modelBuilder.Entity<League>()
                .HasIndex(x => x.Code)
                .IsUnique();

            //modelBuilder.Entity<League>()
            //    .HasKey(x => x.Code);

            modelBuilder.Entity<League>()
                .HasMany(l => l.Teams)
                .WithOne(l => l.League)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlayerTeam>()
                .HasKey(pt => new {pt.PlayerID, pt.TeamID});

     //       modelBuilder.Entity<PlayerTeam>()
     //.HasOne(pc => pc.Player)
     //.WithMany(c => c.PlayerTeams)
     //.HasForeignKey(pc => pc.TeamID)
     //.OnDelete(DeleteBehavior.Restrict);

        }
    }
}
