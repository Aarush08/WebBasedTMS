using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class CourseParticipants
    {
        public int Id { get; set; }
        public DateTime RegTime { get; set; }
        [Required]
        public string Name { get; set; }
        [Required] 
        public string MobileNumber { get; set; }
        [Required] 
        public string EmailAddress { get; set; }
        [Required] 
        public string Module { get; set; }
        public DateTime StartTime { get; set; }
        public bool PaymentStatus { get; set; }
        public string PaymentId { get; set; }
    }
}