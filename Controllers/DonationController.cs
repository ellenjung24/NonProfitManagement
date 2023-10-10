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
    public class DonationController : Controller
    {
        // private readonly ApplicationDbContext _context;

        // public DonationController(ApplicationDbContext context) {
        //     _context = context;
        // }

        public IActionResult Index()
        {
            // var data =  _context.Donations.ToList();
            return View();
        }
    }
}