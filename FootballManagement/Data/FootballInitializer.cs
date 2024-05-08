using FootballManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Numerics;

namespace FootballManagement.Data
{
    public class FootballInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            FootballDbContext context = applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<FootballDbContext>();
            try
            {
                //Delete the database if you need to apply a new Migration
                //context.Database.EnsureDeleted();
                //Create the database if it does not exist and apply the Migration
                context.Database.Migrate();

                // Look for any Doctors.  Since we can't have patients without Doctors.
                
                if (!context.Leagues.Any())
                {
                    context.Leagues.AddRange(
                    new League
                    {
                        Code = "PL",
                        Name = "Premier League"

                    },
                    new League
                    {
                        Code = "LL",
                        Name = "La Liga"

                    },
                    new League
                    {
                        Code = "BL",
                        Name = "Bundesliga"

                    },
                    new League
                    {
                        Code = "FL",
                        Name = "French League"

                    });
                    context.SaveChanges();
                }
                if (!context.Teams.Any())
                {
                    context.Teams.AddRange(
                        new Team
                        {
                            Name = "Real Madrid",
                            LeagueCode = context.Leagues.FirstOrDefault(t=>t.Name == "La Liga").Code,
                            Budget = 9000.00,
                        },
                        new Team
                        {
                            Name = "Villareal",
                            LeagueCode = context.Leagues.FirstOrDefault(t => t.Name == "La Liga").Code,
                            Budget = 5000.00,
                        },
                        new Team
                        {
                            Name = "Atletico Madrid",
                            LeagueCode = context.Leagues.FirstOrDefault(t => t.Name == "La Liga").Code,
                            Budget = 8000.00,
                        },
                    new Team
                    {
                        Name = "Barcelona",
                        LeagueCode = context.Leagues.FirstOrDefault(t => t.Name == "La Liga").Code,
                        Budget = 8000.00,
                    },
                    new Team
                    {
                        Name = "Manchester United",
                        LeagueCode = context.Leagues.FirstOrDefault(t => t.Name == "Premier League").Code,
                        Budget = 8000.00,
                    },
                    new Team
                    {
                        Name = "Bayern Munich",
                        LeagueCode = context.Leagues.FirstOrDefault(t => t.Name == "Bundesliga").Code,
                        Budget = 7000.00,
                    },
                    new Team
                    {
                        Name = "Paris Saint-Germain",
                        LeagueCode = context.Leagues.FirstOrDefault(t => t.Name == "French League").Code,
                        Budget = 8500.00,
                    });
                    context.SaveChanges();
                }
                if (!context.Players.Any())
                {
                    context.Players.AddRange(
                     new Player
                     {
                         FirstName = "Lionel",
                         LastName = "Messi",
                         Jersey = "30",
                         DOB = DateTime.Parse("1986-06-22"),
                         FeePaid = 192.99,
                         EMail = "leomessi@gmail.com",
                         TeamID = context.Teams.FirstOrDefault(t => t.Name == "Paris Saint-Germain").Id

                     },
                     new Player
                     {
                         FirstName = "Ousmane",
                         LastName = "Dembele",
                         Jersey = "11",
                         DOB = DateTime.Parse("2000-06-22"),
                         FeePaid = 192.99,
                         EMail = "dembele@gmail.com",
                         TeamID = context.Teams.FirstOrDefault(t => t.Name == "Barcelona").Id

                     },
                     new Player
                     {
                         FirstName = "Marcus",
                         LastName = "Rashford",
                         Jersey = "10",
                         DOB = DateTime.Parse("2003-06-22"),
                         FeePaid = 192.99,
                         EMail = "rashford@gmail.com",
                         TeamID = context.Teams.FirstOrDefault(t => t.Name == "Manchester United").Id

                     },
                     new Player
                     {
                         FirstName = "Joshua",
                         LastName = "Kimmich",
                         Jersey = "30",
                         DOB = DateTime.Parse("1992-06-22"),
                         FeePaid = 192.99,
                         EMail = "kimmich@gmail.com",
                         TeamID = context.Teams.FirstOrDefault(t => t.Name == "Bayern Munich").Id

                     },
                     new Player
                     {
                         FirstName = "Antoine",
                         LastName = "Griezmann",
                         Jersey = "08",
                         DOB = DateTime.Parse("1990-06-22"),
                         FeePaid = 192.99,
                         EMail = "a.grizzo@gmail.com",
                         TeamID = context.Teams.FirstOrDefault(t => t.Name == "Atletico Madrid").Id

                     },
                     new Player
                     {
                         FirstName = "Frenkie",
                         LastName = "De Jong",
                         Jersey = "21",
                         DOB = DateTime.Parse("1999-06-22"),
                         FeePaid = 192.99,
                         EMail = "frenkie@gmail.com",
                         TeamID = context.Teams.FirstOrDefault(t => t.Name == "Barcelona").Id

                     },
                     new Player
                     {
                         FirstName = "Manuel",
                         LastName = "Nuer",
                         Jersey = "01",
                         DOB = DateTime.Parse("1986-06-22"),
                         FeePaid = 192.99,
                         EMail = "mnuer@gmail.com",
                         TeamID = context.Teams.FirstOrDefault(t => t.Name == "Bayern Munich").Id

                     },

                     new Player
                     {
                         FirstName = "Cristiano",
                         LastName = "Ronaldo",
                         Jersey = "07",
                         DOB = DateTime.Parse("1983-02-22"),
                         FeePaid = 193.99,
                         EMail = "c.ronaldo@gmail.com",
                         TeamID = context.Teams.FirstOrDefault(t => t.Name == "Manchester United").Id

                     },
                     new Player
                     {
                         FirstName = "Neymar",
                         LastName = "Junior",
                         Jersey = "10",
                         DOB = DateTime.Parse("1992-02-05"),
                         FeePaid = 222.99,
                         EMail = "neymar@gmail.com",
                         TeamID = context.Teams.FirstOrDefault(t => t.Name == "Paris Saint-Germain").Id
                     },
                     new Player
                     {
                         FirstName = "Robert",
                         LastName = "Lewandowski",
                         Jersey = "9",
                         DOB = DateTime.Parse("1988-08-21"),
                         FeePaid = 162.99,
                         EMail = "lewandowski@gmail.com",
                         TeamID = context.Teams.FirstOrDefault(t => t.Name == "Barcelona").Id
                     },
                     new Player
                     {
                         FirstName = "Luka",
                         LastName = "Modric",
                         Jersey = "10",
                         DOB = DateTime.Parse("1985-09-09"),
                         FeePaid = 132.99,
                         EMail = "modric@gmail.com",
                         TeamID = context.Teams.FirstOrDefault(t => t.Name == "Real Madrid").Id
                     },
                     new Player
                     {
                         FirstName = "Sergio",
                         LastName = "Ramos",
                         Jersey = "4",
                         DOB = DateTime.Parse("1986-03-30"),
                         FeePaid = 142.99,
                         EMail = "ramos@gmail.com",
                         TeamID = context.Teams.FirstOrDefault(t => t.Name == "Real Madrid").Id
                     },
                     new Player
                     {
                         FirstName = "Kyllian",
                         LastName = "Mbappe",
                         Jersey = "10",
                         DOB = DateTime.Parse("2004-03-12"),
                         FeePaid = 182.99,
                         EMail = "mbappe@gmail.com",
                         TeamID = context.Teams.FirstOrDefault(t => t.Name == "Paris Saint-Germain").Id

                     });
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetBaseException().Message);
            }
        }
    }
}

