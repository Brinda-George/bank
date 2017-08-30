using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public class Account
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
        public int AccountNumber { get; set; }
        [Required]
        public string AccountType { get; set; }
    }
}