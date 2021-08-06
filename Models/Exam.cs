using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class Exam
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Enter the Activity")]
        public string Activity { get; set; }
        [Required(ErrorMessage = "Enter the Day")]
        public string Day { get; set; }
        [Required(ErrorMessage = "Enter the Date")]
        public DateTime Date { get; set; }
    }
}