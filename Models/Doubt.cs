using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class Doubt
    {
        public int Id { get; set; }
        [Required]
        public string EmailId { get; set; }
        public DateTime TimeStamp { get; set; }
        [Required]
        public string Module { get; set; }
        [Required]
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool Status { get; set; }
    }
}