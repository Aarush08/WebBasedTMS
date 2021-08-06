using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class FindParticipants
    {
        [Required]
        [Display(Name="Application Number")]
        public int AppNo { get; set; }
        [Required]
        public string Name { get; set; }
    }
}