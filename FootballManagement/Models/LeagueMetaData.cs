using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FootballManagement.Models
{
    public class LeagueMetaData
    {

        [Display(Name = "Name")]
        [Required(ErrorMessage = "You cannot leave the name blank.")]
        [StringLength(30, ErrorMessage = "Name cannot be more than 30 characters long.")]
        public string Name { get; set; }

    }
}
