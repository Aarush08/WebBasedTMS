using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class LiveStreaming
    {
        public int Id { get; set; }
        [Required]
        public string Module { get; set; }
        [Required]
        [Display(Name ="Meeting Id")]
        public string MeetingId { get; set; }
    }
}