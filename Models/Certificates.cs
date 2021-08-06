using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class Certificates
    {
        public int Id { get; set; }
        [Display(Name="Application Number")]
        public int ApplicationId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Module { get; set; }
        public string Instructor { get; set; }
        public string EmailId { get; set; }
        public DateTime IssuedDate { get; set; }
        public int Score { get; set; }
    }
}