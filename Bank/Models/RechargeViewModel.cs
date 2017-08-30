using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public class RechargeViewModel
    {
        [Required]
        [Display(Name = "Mobile Number")]
        public string mobileNo { get; set; }
        [Required]
        [Display(Name = "Your Account Number")]
        public int accountnumber { get; set; }
        [Required]
        [Display(Name = "Recharge Amount")]
        public decimal amount { get; set; }
    }
}