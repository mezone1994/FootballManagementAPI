using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FootballManagement.Models
{
    [ModelMetadataType(typeof(LeagueMetaData))]
    public class LeagueDTO
    {
        [Key]
        [StringLength(2, ErrorMessage = "The {0} must be exactly 2 characters long.")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "The {0} must be a valid alphabetic string.")]
        public string Code { get; set; }

        public string Name { get; set; }

        public ICollection<TeamDTO> Teams { get; set; }
    }
}
