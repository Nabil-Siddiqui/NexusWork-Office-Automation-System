using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusWork.Data;

namespace NexusWork.Controllers
{
    [Authorize(Roles = "Admin")]  // ONLY Admin can access this controller
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            // Get counts for dashboard
            ViewBag.TotalEmployees = await _context.Employees.CountAsync();
            ViewBag.ActiveEmployees = await _context.Employees.CountAsync(e => e.IsActive);
            ViewBag.TotalTasks = await _context.WorkTasks.CountAsync();
            ViewBag.PendingTasks = await _context.WorkTasks.CountAsync(t => t.Status == "Pending");
            ViewBag.InProgressTasks = await _context.WorkTasks.CountAsync(t => t.Status == "In Progress");
            ViewBag.CompletedTasks = await _context.WorkTasks.CountAsync(t => t.Status == "Completed");
            ViewBag.TotalDocuments = await _context.Documents.CountAsync();

            // Get recent tasks
            ViewBag.RecentTasks = await _context.WorkTasks
                .Include(t => t.Employee)
                .OrderByDescending(t => t.AssignedDate)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }
}