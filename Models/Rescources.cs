using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class Rescources
    {
        public int Id { get; set; }
        [Required]
        public string Module { get; set; }
        [Required] 
        public string ResourceType { get; set; }
        [Required]
        [Display(Name ="Upload Files")]
        public string FileName { get; set; }
    }
}