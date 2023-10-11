using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NonProfitManagement.Models
{
    public class PaymentMethod
    {
        [Key]
        [Display(Name = "Payment Method Id")]
        public int PaymentMethodId { get; set; }
        [Display(Name = "Payment Method")]
        public string? Name { get; set; }
    }
}