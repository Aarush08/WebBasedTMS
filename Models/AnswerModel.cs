using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class AnswerModel
    {
        public int Id { get; set; }
        public int ApplicationNo { get; set; }
        public int QuesNo { get; set; }
        public string ChoiceQue { get; set; }
    }
}