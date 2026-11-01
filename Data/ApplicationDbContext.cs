using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NexusWork.Models;

namespace NexusWork.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<WorkTask> WorkTasks { get; set; }
        public DbSet<PayrollEntry> PayrollEntries { get; set; }
        public DbSet<Document> Documents { get; set; }
    }
}
