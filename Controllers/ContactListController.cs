using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NonProfitManagement.Data;

namespace NonProfitManagement.Controllers
{
    public class ContactListController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ContactListController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index()
        // public IActionResult Index()
        {
            var data = await _context.ContactLists.ToListAsync();
            // var data = _context.ContactLists.ToList();
            return View();
        }
    }
}