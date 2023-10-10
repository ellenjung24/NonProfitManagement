using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NonProfitManagement.Models;

namespace NonProfitManagement.Views.Donation
{
    public class Index : PageModel
    {
        private readonly NonProfitManagement.Data.ApplicationDbContext _context;

        public Index(NonProfitManagement.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<NonProfitManagement.Models.Donation> Donation { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Donations != null)
            {
                Donation = await _context.Donations.ToListAsync();
            }
        }
    }
}