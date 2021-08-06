using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class TestCentres
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Enter the Centre Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter the Centre Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Enter the Centre City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Enter the State & Country")]
        [Display(Name = "State & Country")]
        public string State { get; set; }

        [Display(Name = "Pin Code")]
        public int PinCode { get; set; }
        [Required(ErrorMessage = "Enter the Centre Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter the Phone Number")]
        [Display(Name = "Phone Number")]
        public string ContactNo { get; set; }

        [Display(Name = "Total Seats")]
        public int TotalSeats { get; set; }

        [Display(Name = "Available Seats")]
        public int AvailableSeats { get; set; }
    }
}