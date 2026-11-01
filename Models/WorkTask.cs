using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusWork.Models
{
    public class WorkTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Task title is required")]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Assigned To")]
        public int EmployeeId { get; set; }

        [Display(Name = "Assigned Date")]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

        [Required]
        public string Priority { get; set; } = "Medium";  // Low, Medium, High

        [Required]
        public string Status { get; set; } = "Pending";  // Pending, In Progress, Completed

        // Navigation Property
        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        // Computed Properties
        [NotMapped]
        public bool IsOverdue => Status != "Completed" && DateTime.Now > Deadline;

        [NotMapped]
        public string PriorityBadgeClass => Priority switch
        {
            "High" => "bg-danger",
            "Medium" => "bg-warning",
            "Low" => "bg-secondary",
            _ => "bg-secondary"
        };

        [NotMapped]
        public string StatusBadgeClass => Status switch
        {
            "Completed" => "bg-success",
            "In Progress" => "bg-primary",
            "Pending" => "bg-secondary",
            _ => "bg-secondary"
        };
    }
}
