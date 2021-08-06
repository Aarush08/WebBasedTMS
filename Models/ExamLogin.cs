using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class ExamLogin
    {
        [Required]
        [Display(Name="Enter the Application Number")]
        public int ApplicationNo { get; set; }
        [Required]
        [Display(Name = "Enter the Password")]
        public string Password { get; set; }
    }
}