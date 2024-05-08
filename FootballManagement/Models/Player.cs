﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace FootballManagement.Models
{
    [ModelMetadataType(typeof(PlayerMetaData))]
    public class Player : Auditable, IValidatableObject
    {
        public int ID { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }


        public string FormalName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int a = today.Year - DOB.Year
                    - ((today.Month < DOB.Month || (today.Month == DOB.Month && today.Day < DOB.Day) ? 1 : 0));
                return a; /*Note: You could add .PadLeft(3) but spaces disappear in a web page. */
            }
        }

     
        public string FirstName { get; set; }

     
        public string LastName { get; set; }

       
        public string Jersey { get; set; }

        public DateTime DOB { get; set; }

        public double FeePaid { get; set; }

        public string EMail { get; set; }

        public int TeamID { get; set; }

        public Team Team { get; set; }

        [Timestamp]
        public Byte[] RowVersion { get; set; }//Added for concurrency

        public ICollection<PlayerTeam> PlayerTeams { get; set; }

        //public ICollection<Team> Teams { get; set; } = new HashSet<Team>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Age > 10 && FeePaid < 120.0)
            {
                yield return new ValidationResult("Players over 10 years old must pay a Fee of at least $120.", new[] { "FeePaid" });
            }
        }
    }
}
