using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bank.Models
{
    public class TransferViewModel
    {
        [Required]
        [Display(Name = "Sender's Account Number")]
        public int SenderAccountNumber { get; set; }
        [Required]
        [Display(Name = "Receiver's Account Number")]
        public int ReceiverAccountNumber { get; set; }
        [Required]
        [Display(Name = "Amount To Transfer")]
        public decimal TransferAmount { get; set; }
    }
}