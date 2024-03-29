using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NonProfitManagement.Data;
using NonProfitManagement.Models;

namespace NonProfitManagement.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Donation
        [Authorize(Roles = "Admin, Finance")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Donations.Include(d => d.ContactList).Include(d => d.PaymentMethod).Include(d => d.TransactionType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Donation/Details/5
        [Authorize(Roles = "Admin, Finance")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Donations == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.ContactList)
                .Include(d => d.PaymentMethod)
                .Include(d => d.TransactionType)
                .FirstOrDefaultAsync(m => m.TransId == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // GET: Donation/Create
        [Authorize(Roles = "Admin, Finance")]
        public IActionResult Create()
        {
            ViewData["AccountNo"] = new SelectList(_context.ContactLists, "AccountNo", "Email");
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "Name");
            ViewData["TransactionTypeId"] = new SelectList(_context.TransactionTypes, "TransactionTypeId", "Name");
            return View();
        }

        // POST: Donation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Finance")]
        public async Task<IActionResult> Create([Bind("TransId,Date,AccountNo,TransactionTypeId,Amount,PaymentMethodId,Notes")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                donation.Created = DateTime.Now;
                donation.CreatedBy = User.Identity.Name;
                _context.Add(donation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountNo"] = new SelectList(_context.ContactLists, "AccountNo", "City", donation.AccountNo);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "Name", donation.PaymentMethodId);
            ViewData["TransactionTypeId"] = new SelectList(_context.TransactionTypes, "TransactionTypeId", "Description", donation.TransactionTypeId);
            return View(donation);
        }

        // GET: Donation/Edit/5
        [Authorize(Roles = "Admin, Finance")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Donations == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            ViewData["AccountNo"] = new SelectList(_context.ContactLists, "AccountNo", "City", donation.AccountNo);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "Name", donation.PaymentMethodId);
            ViewData["TransactionTypeId"] = new SelectList(_context.TransactionTypes, "TransactionTypeId", "Description", donation.TransactionTypeId);
            return View(donation);
        }

        // POST: Donation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Finance")]
        public async Task<IActionResult> Edit(int id, [Bind("TransId,Date,AccountNo,TransactionTypeId,Amount,PaymentMethodId,Notes")] Donation donation)
        {
            if (id != donation.TransId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var originalDonation = await _context.Donations.AsNoTracking().FirstOrDefaultAsync(d => d.TransId == id);
                if (originalDonation == null)
                {
                    return NotFound();
                }
                try
                {
                    donation.Created = originalDonation.Created;
                    donation.CreatedBy = originalDonation.CreatedBy;
                    donation.Modified = DateTime.Now;
                    donation.ModifiedBy = User.Identity.Name;
                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationExists(donation.TransId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountNo"] = new SelectList(_context.ContactLists, "AccountNo", "City", donation.AccountNo);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "Name", donation.PaymentMethodId);
            ViewData["TransactionTypeId"] = new SelectList(_context.TransactionTypes, "TransactionTypeId", "Description", donation.TransactionTypeId);
            return View(donation);
        }

        // GET: Donation/Delete/5
        [Authorize(Roles = "Admin, Finance")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Donations == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.ContactList)
                .Include(d => d.PaymentMethod)
                .Include(d => d.TransactionType)
                .FirstOrDefaultAsync(m => m.TransId == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // POST: Donation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Finance")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Donations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Donations'  is null.");
            }
            var donation = await _context.Donations.FindAsync(id);
            if (donation != null)
            {
                _context.Donations.Remove(donation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonationExists(int id)
        {
          return (_context.Donations?.Any(e => e.TransId == id)).GetValueOrDefault();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> YtdReport() {
            var donationList = await _context.Donations
                .Where(m => m.Date.Value.Year == DateTime.Now.Year).ToListAsync();
            if (donationList == null)
            {
                return NotFound();
            }
            float? total = 0;
            // List<String> nameList = new List<String>();
            foreach (Donation item in donationList)
            {
                total += item.Amount;
            }
            ViewBag.ytdTotal = total;
            ViewBag.today = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.thisYear = DateTime.Now.Year;
            // ViewBag.nameList = nameList;
            // return View(donationList);

            var currentYear = DateTime.Now.Year;
            var startDate = new DateTime(currentYear, 1, 1);
            var endDate = DateTime.Now;

            var donations = await _context.Donations
                .Include(d => d.ContactList)
                .Where(d => d.Date >= startDate && d.Date <= endDate)
                .GroupBy(d => new { d.ContactList.FirstName, d.ContactList.LastName })
                .Select(g => new              {
                    FirstName = g.Key.FirstName,
                    LastName = g.Key.LastName,
                    TotalAmount = g.Sum(d => d.Amount)
                })
                .ToListAsync();

            if (donations == null || !donations.Any())
            {
                return NotFound();
            }

            return View(donations);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> YearlyReport() {
            // var donationList = await _context.Donations
            //     .Where(m => m.Date.Value.Year == DateTime.Now.Year).ToListAsync();
            var donationList = await _context.Donations.ToListAsync();
            if (donationList == null)
            {
                return NotFound();
            }


              var groupedDonations = donationList
                .GroupBy(d => d.Date.Value.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    Donations = g.ToList(),
                    TotalAmount = g.Sum(donation => donation.Amount)
                })
                .OrderByDescending(g => g.Year);


                // .Where(m => m.AccountNo == id && m.Date.Value.Year == currentYear).ToListAsync();
                if (donationList == null)
                {
                    Console.WriteLine("donationList is null");
                    return NotFound();
                }
            float? total = 0;
            // List<String> nameList = new List<String>();
            foreach (Donation item in donationList)
            {
                total += item.Amount;
            }
            ViewBag.ytdTotal = total;
            ViewBag.today = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.thisYear = DateTime.Now.Year;
            // ViewBag.nameList = nameList;
            // return View(donationList);

          

            return View(groupedDonations);
        }

      
    }
}
