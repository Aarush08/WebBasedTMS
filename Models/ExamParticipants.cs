using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class ExamParticipants
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name= "Mobile Number")]
        public string MobileNumber { get; set; }
        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        public bool DetailsConfirm { get; set; }
        [Required]
        [Display(Name = "Date Of Birth")]
        public DateTime DateofBirth { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public int PinCode { get; set; }
        [Required]
        [Display(Name = "Passport Size Image")]
        public string Image { get; set; }
        [Required]
        public string Module { get; set; }
        [Required]
        public DateTime ExamDate { get; set; }
        [Required]
        [Display(Name = "Exam City")]
        public string ExamCity { get; set; }
        public bool PaymmentStatus { get; set; }
        public string PaymentId { get; set; }

    }
}