using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using NonProfitManagement.Data;
using NonProfitManagement.Models;

namespace NonProfitManagement.Controllers
{
    public class ContactListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContactList
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return _context.ContactLists != null ?
                        View(await _context.ContactLists.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.ContactLists'  is null.");
        }

        // GET: ContactList/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ContactLists == null)
            {
                return NotFound();
            }

            var contactList = await _context.ContactLists
                .FirstOrDefaultAsync(m => m.AccountNo == id);
            if (contactList == null)
            {
                return NotFound();
            }

            return View(contactList);
        }

        // GET: ContactList/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ShowError() {
            return View();
        }

        // POST: ContactList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("AccountNo,FirstName,LastName,Email,Street,City,PostalCode,Country")] ContactList contactList)
        {
            if (ModelState.IsValid)
            {
                contactList.CreatedBy = User.Identity.Name;
                contactList.Created = DateTime.Now;
                _context.Add(contactList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactList);
        }

        // GET: ContactList/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ContactLists == null)
            {
                return NotFound();
            }

            var contactList = await _context.ContactLists.FindAsync(id);
            if (contactList == null)
            {
                return NotFound();
            }
            return View(contactList);
        }

        // POST: ContactList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("AccountNo,FirstName,LastName,Email,Street,City,PostalCode,Country")] ContactList contactList)
        {
            if (id != contactList.AccountNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var contactListDb = await _context.ContactLists.AsNoTracking().FirstOrDefaultAsync(m => m.AccountNo == id);
                if (contactListDb == null)
                {
                    return NotFound();
                }
                try
                {
                    contactList.Created = contactListDb.Created;
                    contactList.CreatedBy = contactListDb.CreatedBy;
                    contactList.Modified = DateTime.Now;
                    contactList.ModifiedBy = User.Identity.Name;
                    _context.Update(contactList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactListExists(contactList.AccountNo))
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
            return View(contactList);
        }

        // GET: ContactList/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ContactLists == null)
            {
                return NotFound();
            }

            var contactList = await _context.ContactLists
                .FirstOrDefaultAsync(m => m.AccountNo == id);
            if (contactList == null)
            {
                return NotFound();
            }

            return View(contactList);
        }

        // POST: ContactList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ContactLists == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ContactLists'  is null.");
            }
            var contactList = await _context.ContactLists.FindAsync(id);
            if (contactList != null)
            {
                _context.ContactLists.Remove(contactList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        private bool ContactListExists(int id)
        {
            return (_context.ContactLists?.Any(e => e.AccountNo == id)).GetValueOrDefault();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Receipt(int? id)
        {
            try {
                if (id == null || _context.ContactLists == null)
                {
                    return NotFound();
                }
                // View(await _context.ContactLists.ToListAsync())

                var currentYear = DateTime.Now.Year;
                var user = await _context.ContactLists
                    .FirstOrDefaultAsync(m => m.AccountNo == id);
                var donationList = await _context.Donations
                    .Where(m => m.AccountNo == id && m.Date.Value.Year == currentYear).ToListAsync();
                if (donationList == null)
                {
                    Console.WriteLine("donationList is null");
                    return NotFound();
                }
                ViewBag.userName = user.FirstName + " " + user.LastName;
                if (donationList[0].Date != null)
                {
                    ViewBag.thisYear = donationList[0].Date.Value.Year;
                }
                // ViewBag.thisYear = donationList[0].Date.

                float? total = 0;
                foreach (Donation item in donationList)
                {
                    total += item.Amount;
                }
                ViewBag.totalAmount = total;

                return View(donationList);

            } catch (Exception ex) {
                // return View('ShowError');
                return RedirectToAction(nameof(ShowError));
            }

        }
    }
}
