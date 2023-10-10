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
        public int PaymentMethodId { get; set; }
        public string? Name { get; set; }
    }
}