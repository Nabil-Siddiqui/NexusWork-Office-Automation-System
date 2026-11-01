using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusWork.Models
{
    public class Employee
    {
        public int Id { get; set; }

        // Personal Information
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }  // Male, Female, Other

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "NID is required")]
        [StringLength(17)]
        [Display(Name = "National ID (NID)")]
        public string NationalId { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        // Employment Information
        [Required]
        [StringLength(20)]
        [Display(Name = "Employee ID")]
        public string EmployeeCode { get; set; }  // Example: NW-001

        [Required]
        public string Department { get; set; }

        [Required]
        public string Designation { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // Salary Information
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Basic Salary (BDT)")]
        public decimal BasicSalaryBDT { get; set; }

        // Bank Information
        [Required]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }  // Dropdown selection

        [Required]
        [Display(Name = "Account Number")]
        public string BankAccountNumber { get; set; }

        // Navigation Properties
        public ICollection<WorkTask>? AssignedTasks { get; set; }
        public ICollection<PayrollEntry>? PayrollEntries { get; set; }

        // Computed Property
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
