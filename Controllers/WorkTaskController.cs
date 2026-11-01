using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NexusWork.Data;
using NexusWork.Models;

namespace NexusWork.Controllers
{
    [Authorize]  // Must be logged in
    public class WorkTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkTask
        public async Task<IActionResult> Index()
        {
            var tasks = _context.WorkTasks.Include(w => w.Employee);

            // If user is Admin, show all tasks
            // If user is Employee, show only their tasks
            if (User.IsInRole("Admin"))
            {
                return View(await tasks.ToListAsync());
            }
            else
            {
                // Get current user's email
                var userEmail = User.FindFirstValue(ClaimTypes.Email);

                // Find employee with matching email
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == userEmail);

                if (employee != null)
                {
                    var myTasks = await tasks.Where(t => t.EmployeeId == employee.Id).ToListAsync();
                    return View(myTasks);
                }

                // If no matching employee found, show empty list
                return View(new List<WorkTask>());
            }
        }

        // GET: WorkTask/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workTask = await _context.WorkTasks
                .Include(w => w.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workTask == null)
            {
                return NotFound();
            }

            // Check if employee can view this task
            if (!User.IsInRole("Admin"))
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == userEmail);

                if (employee == null || workTask.EmployeeId != employee.Id)
                {
                    return Forbid();
                }
            }

            return View(workTask);
        }

        // GET: WorkTask/Create
        [Authorize(Roles = "Admin")]  // Only Admin can create tasks
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["Priorities"] = new SelectList(DropdownData.TaskPriorities);
            ViewData["Statuses"] = new SelectList(DropdownData.TaskStatuses);
            return View();
        }

        // POST: WorkTask/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,EmployeeId,AssignedDate,Deadline,Priority,Status")] WorkTask workTask)
        {
            if (ModelState.IsValid)
            {
                workTask.AssignedDate = DateTime.Now;
                _context.Add(workTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", workTask.EmployeeId);
            ViewData["Priorities"] = new SelectList(DropdownData.TaskPriorities, workTask.Priority);
            ViewData["Statuses"] = new SelectList(DropdownData.TaskStatuses, workTask.Status);
            return View(workTask);
        }

        // GET: WorkTask/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workTask = await _context.WorkTasks.FindAsync(id);
            if (workTask == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", workTask.EmployeeId);
            ViewData["Priorities"] = new SelectList(DropdownData.TaskPriorities, workTask.Priority);
            ViewData["Statuses"] = new SelectList(DropdownData.TaskStatuses, workTask.Status);
            return View(workTask);
        }

        // POST: WorkTask/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,EmployeeId,AssignedDate,Deadline,Priority,Status")] WorkTask workTask)
        {
            if (id != workTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkTaskExists(workTask.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", workTask.EmployeeId);
            ViewData["Priorities"] = new SelectList(DropdownData.TaskPriorities, workTask.Priority);
            ViewData["Statuses"] = new SelectList(DropdownData.TaskStatuses, workTask.Status);
            return View(workTask);
        }

        // GET: WorkTask/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workTask = await _context.WorkTasks
                .Include(w => w.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workTask == null)
            {
                return NotFound();
            }

            return View(workTask);
        }

        // POST: WorkTask/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workTask = await _context.WorkTasks.FindAsync(id);
            if (workTask != null)
            {
                _context.WorkTasks.Remove(workTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkTaskExists(int id)
        {
            return _context.WorkTasks.Any(e => e.Id == id);
        }
    }
}