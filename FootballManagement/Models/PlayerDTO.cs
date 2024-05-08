using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace FootballManagement.Models
{
    [ModelMetadataType(typeof(PlayerMetaData))]
    public class PlayerDTO : IValidatableObject
    {
        public int ID { get; set; }

     
        public string FirstName { get; set; }

     
        public string LastName { get; set; }

       
        public string Jersey { get; set; }

        public DateTime DOB { get; set; }

        public double FeePaid { get; set; }

        public string EMail { get; set; }

        public int TeamID { get; set; }

        public TeamDTO Team { get; set; }

        public int? NumberOfTeams { get; set; } = null;

        [Timestamp]
        public Byte[] RowVersion { get; set; }//Added for concurrency

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DOB > DateTime.Today.AddYears(-10) && FeePaid < 120.0)
            {
                yield return new ValidationResult("Players over 10 years old must pay a Fee of at least $120.", new[] { "FeePaid" });
            }
        }
    }
}
