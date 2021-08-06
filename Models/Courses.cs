using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class Courses
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Enter the Module")]
        public string Module { get; set; }
        [Required(ErrorMessage = "Enter Start Date")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Enter End Date")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Enter Duration")]
        public int Duration { get; set; }
        public string Content { get; set; }
        public Trainers Trainers { get; set; }
        public int TrainersId { get; set; }
    }
}