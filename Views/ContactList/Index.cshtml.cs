using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NonProfitManagement.Models;
using NonProfitManagement.Data;

namespace NonProfitManagement.Views.ContactList
{
    public class Index : PageModel
    {
        private readonly NonProfitManagement.Data.ApplicationDbContext _context;

        public Index(NonProfitManagement.Data.ApplicationDbContext context) {
            _context = context;
        }

        public IList<NonProfitManagement.Models.ContactList> ContactList { get;set; } = default!;

        public async Task OnGetAsync() {
            if (_context.ContactLists != null) {
                ContactList = await _context.ContactLists.ToListAsync();
            }
        }
    }
}