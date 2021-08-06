using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class TestPapers
    {
        public int Id { get; set; }
        public string Module { get; set; }
        public string Question { get; set; }
        public string Choice1 { get; set; }
        public string Choice2 { get; set; }
        public string Choice3 { get; set; }
        public string Choice4 { get; set; }
        public string CorrectAnswer { get; set; }
    }
}