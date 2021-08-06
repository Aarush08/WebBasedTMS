using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class AdmitCard
    {
        public int Id { get; set; }
        public int ApplicationNo { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Image { get; set; }
        public DateTime DateOfExam { get; set; }
        public string ExamCentre { get; set; }
        public string Module { get; set; }
    }
}