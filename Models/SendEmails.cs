using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class SendEmails
    {
        [Required]
        public string ToWhom { get; set; }
        [Required(ErrorMessage ="Enter the Subject")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Enter the Message")]
        public string Body { get; set; }
    }
}