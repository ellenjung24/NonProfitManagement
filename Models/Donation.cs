using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NonProfitManagement.Models
{
    public class Donation
    {
        [Key]
        public int TransId { get; set; }
        public DateTime? Date { get; set; }
        public int? AccountNo { get; set; }
        [ForeignKey("AccountNo")]
        public ContactList? ContactList { get; set; }
        public int? TransactionTypeId { get; set; }
        [ForeignKey("TransactionTypeId")]
        public TransactionType? TransactionType { get; set; }
        public float? Amount { get; set; }
        public int? PaymentMethodId { get; set; }
        [ForeignKey("PaymentMethodId")]
        public PaymentMethod? PaymentMethod { get; set; }
        public string? Notes { get; set; }

    }
}