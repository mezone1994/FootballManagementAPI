using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballManagement.Models
{
    [ModelMetadataType(typeof(TeamMetaData))]
    public class Team : Auditable, IValidatableObject
    {
        public int Id { get; set; }

       
        public string Name { get; set; }

        public double Budget { get; set; }

        public string LeagueCode { get; set; }

        public League League { get; set; }

        public ICollection<Player> Players { get; set; } 


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (Name[0] == 'X' || Name[0] == 'F' || Name[0] == 'S')
            {
                yield return new ValidationResult("Team names are not allowed to start with the letters X, F, or S.", new[] { "Name" });
            }
        }
    }
}
