using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class Feedbacks
    {
        public int Id { get; set; }
        [Required]
        public string Module { get; set; }
        [Required]
        public string Faculty { get; set; }
        public int Q1 { get; set; }
        public int Q2 { get; set; }
        public int Q3 { get; set; }
        public int Q4 { get; set; }
        public int Q5 { get; set; }
        public string Suggestions { get; set; }
    }
}