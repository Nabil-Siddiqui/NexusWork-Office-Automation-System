using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusWork.Data;
using NexusWork.Models;
using System.Security.Claims;

namespace NexusWork.Controllers
{
    [Authorize]
    public class EmployeeDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get current user's email
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            // Find employee with matching email
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == userEmail);

            if (employee == null)
            {
                // User is registered but not added as employee by admin
                ViewBag.NotLinked = true;
                ViewBag.UserEmail = userEmail;
                return View();
            }

            // Get employee's data
            ViewBag.Employee = employee;
            ViewBag.NotLinked = false;

            // Get task statistics
            var tasks = _context.WorkTasks.Where(t => t.EmployeeId == employee.Id);
            ViewBag.TotalTasks = await tasks.CountAsync();
            ViewBag.PendingTasks = await tasks.CountAsync(t => t.Status == "Pending");
            ViewBag.InProgressTasks = await tasks.CountAsync(t => t.Status == "In Progress");
            ViewBag.CompletedTasks = await tasks.CountAsync(t => t.Status == "Completed");

            // Get overdue tasks
            ViewBag.OverdueTasks = await tasks
                .CountAsync(t => t.Status != "Completed" && t.Deadline < DateTime.Now);

            // Get recent tasks
            ViewBag.RecentTasks = await tasks
                .OrderByDescending(t => t.AssignedDate)
                .Take(5)
                .ToListAsync();

            // Get latest payroll
            ViewBag.LatestPayroll = await _context.PayrollEntries
                .Where(p => p.EmployeeId == employee.Id)
                .OrderByDescending(p => p.PayYear)
                .ThenByDescending(p => p.PayMonth)
                .FirstOrDefaultAsync();

            return View();
        }

        public async Task<IActionResult> MyProfile()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == userEmail);

            if (employee == null)
            {
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        public async Task<IActionResult> MyPayroll()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == userEmail);

            if (employee == null)
            {
                return RedirectToAction("Index");
            }

            var payrolls = await _context.PayrollEntries
                .Where(p => p.EmployeeId == employee.Id)
                .OrderByDescending(p => p.PayYear)
                .ThenByDescending(p => p.PayMonth)
                .ToListAsync();

            ViewBag.Employee = employee;
            return View(payrolls);
        }
    }
}
