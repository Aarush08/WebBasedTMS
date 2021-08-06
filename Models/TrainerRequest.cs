using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class TrainerRequest
    {

        public int Id { get; set; }
        [Display(Name = "Full Name")]
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }
        [Required]
        public string Qualification { get; set; }
        [Required]
        public int Experience { get; set; }
        [Required]
        public string Skills { get; set; }
        [Required]
        [Display(Name = "Linked Profile")]
        public string SocialProfile { get; set; }
        [Required]
        [Display(Name = "Image Upload")]
        public string ImageName { get; set; }
        [Required]
        [Display(Name = "Signature Upload")]
        public string SignName { get; set; }
    }
}