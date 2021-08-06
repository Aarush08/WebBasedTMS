using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class Scores
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public DateTime DateOfExam { get; set; }
        public string Module { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public int Score { get; set; }
    }
}