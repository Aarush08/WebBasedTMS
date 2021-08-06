using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class SessionData
    {
        public int Id { get; set; }
        public int ApplicationNo { get; set; }
        public int SessionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}