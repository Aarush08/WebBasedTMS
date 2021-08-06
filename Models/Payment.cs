using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedTMS.Models
{
    public class Payment
    {
        [Required] public int Id { get; set; }
        [Required] [Display(Name ="Participant Name")]public string Name { get; set; }
        [Required] public string Reason { get; set; }
        [Required] [Display(Name = "Card Type")] public string CardType { get; set; }
        [Required] [Display(Name = "Card Number")]
        [Range(00,9999999999999999, ErrorMessage = "Enter Valid Card Number")] public string CardNumber { get; set; }
        [Required] [Display(Name = "Expiry Year(YYYY)")]
        [Range(2020,2035, ErrorMessage = "Enter Valid Year")] public int ExpiryYear { get; set; }
        [Required] [Display(Name = "Expiry Month(MM)")]
        [Range(01,12, ErrorMessage = "Enter Valid Month")] public int ExpiryMonth { get; set; }
        [Required] [Display(Name = "CVV")]
        [Range(000,999, ErrorMessage = "Enter Valid CVV")] public int Cvv { get; set; }
        [Required] [Display(Name = "Card Holder Name")] public string CardHolderName { get; set; }
    }
}