using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NonProfitManagement.Models
{
    public class TransactionType
    {
        [Key]
        [Display(Name = "Transaction Type Id")]
        public int TransactionTypeId { get; set; }
        [Display(Name = "Transaction Type")]
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}