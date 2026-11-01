using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NexusWork.Data;
using NexusWork.Models;

namespace NexusWork.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PayrollEntryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PayrollEntryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PayrollEntry
        public async Task<IActionResult> Index()
        {
            var payrollEntries = _context.PayrollEntries
                .Include(p => p.Employee)
                .OrderByDescending(p => p.PayYear)
                .ThenByDescending(p => p.PayMonth);

            return View(await payrollEntries.ToListAsync());
        }

        // GET: PayrollEntry/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payrollEntry = await _context.PayrollEntries
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (payrollEntry == null)
            {
                return NotFound();
            }

            return View(payrollEntry);
        }

        // GET: PayrollEntry/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees.Where(e => e.IsActive), "Id", "FullName");
            ViewData["PayMonths"] = new SelectList(DropdownData.PayMonths);
            return View();
        }

        // POST: PayrollEntry/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,PayMonth,PayYear,BasicSalaryBDT,HouseRentBDT,MedicalAllowanceBDT,TransportAllowanceBDT,BonusBDT,DeductionsBDT,IsPaid")] PayrollEntry payrollEntry)
        {
            if (ModelState.IsValid)
            {
                payrollEntry.GeneratedDate = DateTime.Now;
                _context.Add(payrollEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EmployeeId"] = new SelectList(_context.Employees.Where(e => e.IsActive), "Id", "FullName", payrollEntry.EmployeeId);
            ViewData["PayMonths"] = new SelectList(DropdownData.PayMonths, payrollEntry.PayMonth);
            return View(payrollEntry);
        }

        // GET: PayrollEntry/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payrollEntry = await _context.PayrollEntries.FindAsync(id);

            if (payrollEntry == null)
            {
                return NotFound();
            }

            ViewData["EmployeeId"] = new SelectList(_context.Employees.Where(e => e.IsActive), "Id", "FullName", payrollEntry.EmployeeId);
            ViewData["PayMonths"] = new SelectList(DropdownData.PayMonths, payrollEntry.PayMonth);
            return View(payrollEntry);
        }

        // POST: PayrollEntry/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,PayMonth,PayYear,BasicSalaryBDT,HouseRentBDT,MedicalAllowanceBDT,TransportAllowanceBDT,BonusBDT,DeductionsBDT,IsPaid,GeneratedDate")] PayrollEntry payrollEntry)
        {
            if (id != payrollEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payrollEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayrollEntryExists(payrollEntry.Id))
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

            ViewData["EmployeeId"] = new SelectList(_context.Employees.Where(e => e.IsActive), "Id", "FullName", payrollEntry.EmployeeId);
            ViewData["PayMonths"] = new SelectList(DropdownData.PayMonths, payrollEntry.PayMonth);
            return View(payrollEntry);
        }

        // GET: PayrollEntry/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payrollEntry = await _context.PayrollEntries
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (payrollEntry == null)
            {
                return NotFound();
            }

            return View(payrollEntry);
        }

        // POST: PayrollEntry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payrollEntry = await _context.PayrollEntries.FindAsync(id);

            if (payrollEntry != null)
            {
                _context.PayrollEntries.Remove(payrollEntry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PayrollEntryExists(int id)
        {
            return _context.PayrollEntries.Any(e => e.Id == id);
        }
    }
}