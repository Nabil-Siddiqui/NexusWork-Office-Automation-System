using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusWork.Models
{
    public class Document
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }  // Policy, Handbook, Template, Form

        [Display(Name = "File Type")]
        public string FileType { get; set; } = "PDF";

        [Display(Name = "Upload Date")]
        public DateTime UploadDate { get; set; } = DateTime.Now;

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; } = false;

        [Display(Name = "Status Message")]
        public string StatusMessage { get; set; } = "Document storage not configured. Check back later.";

        // Computed Property
        [NotMapped]
        public string CategoryBadgeClass => Category switch
        {
            "Policy" => "bg-danger",
            "Handbook" => "bg-primary",
            "Template" => "bg-success",
            "Form" => "bg-info",
            _ => "bg-secondary"
        };
    }
}
