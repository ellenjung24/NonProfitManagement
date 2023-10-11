using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NonProfitManagement.Models
{
    public class ContactList
    {
        [Key]
        public int AccountNo { get; set; }
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
}