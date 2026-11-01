using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusWork.Data;
using NexusWork.Models;

namespace NexusWork.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Document
        public IActionResult Index()
        {
            // Create mock documents for display
            var mockDocuments = new List<Document>
            {
                new Document
                {
                    Id = 1,
                    DocumentName = "Employee Handbook 2025",
                    Description = "Complete guide for all employees covering company policies, benefits, and procedures.",
                    Category = "Handbook",
                    FileType = "PDF",
                    UploadDate = new DateTime(2025, 1, 1),
                    IsAvailable = false,
                    StatusMessage = "Document server integration pending."
                },
                new Document
                {
                    Id = 2,
                    DocumentName = "Leave Application Form",
                    Description = "Standard form for requesting annual, sick, or casual leave.",
                    Category = "Form",
                    FileType = "PDF",
                    UploadDate = new DateTime(2025, 1, 5),
                    IsAvailable = false,
                    StatusMessage = "Document server integration pending."
                },
                new Document
                {
                    Id = 3,
                    DocumentName = "IT Security Policy",
                    Description = "Guidelines for maintaining information security and data protection.",
                    Category = "Policy",
                    FileType = "PDF",
                    UploadDate = new DateTime(2025, 1, 10),
                    IsAvailable = false,
                    StatusMessage = "Document server integration pending."
                },
                new Document
                {
                    Id = 4,
                    DocumentName = "Expense Reimbursement Form",
                    Description = "Form for claiming business-related expenses.",
                    Category = "Form",
                    FileType = "PDF",
                    UploadDate = new DateTime(2025, 1, 12),
                    IsAvailable = false,
                    StatusMessage = "Document server integration pending."
                },
                new Document
                {
                    Id = 5,
                    DocumentName = "Code of Conduct",
                    Description = "Professional behavior and ethics guidelines for all staff members.",
                    Category = "Policy",
                    FileType = "PDF",
                    UploadDate = new DateTime(2025, 1, 15),
                    IsAvailable = false,
                    StatusMessage = "Document server integration pending."
                },
                new Document
                {
                    Id = 6,
                    DocumentName = "Onboarding Checklist",
                    Description = "Step-by-step checklist for new employee orientation.",
                    Category = "Template",
                    FileType = "DOCX",
                    UploadDate = new DateTime(2025, 1, 18),
                    IsAvailable = false,
                    StatusMessage = "Document server integration pending."
                },
                new Document
                {
                    Id = 7,
                    DocumentName = "Performance Review Template",
                    Description = "Quarterly performance evaluation form for managers.",
                    Category = "Template",
                    FileType = "XLSX",
                    UploadDate = new DateTime(2025, 1, 20),
                    IsAvailable = false,
                    StatusMessage = "Document server integration pending."
                },
                new Document
                {
                    Id = 8,
                    DocumentName = "Health & Safety Guidelines",
                    Description = "Workplace safety procedures and emergency protocols.",
                    Category = "Policy",
                    FileType = "PDF",
                    UploadDate = new DateTime(2025, 1, 22),
                    IsAvailable = false,
                    StatusMessage = "Document server integration pending."
                }
            };

            return View(mockDocuments);
        }

        // GET: Document/Download/5
        public IActionResult Download(int id)
        {
            // This is a mock - show message instead of actual download
            TempData["Message"] = "Document download feature is not available in demo mode. Cloud storage integration required.";
            TempData["MessageType"] = "warning";
            return RedirectToAction(nameof(Index));
        }
    }
}