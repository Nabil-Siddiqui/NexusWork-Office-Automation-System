using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusWork.Models
{
    public class PayrollEntry
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        // Pay Period
        [Required]
        [Display(Name = "Month")]
        public string PayMonth { get; set; }  // January, February, etc.

        [Required]
        [Display(Name = "Year")]
        public int PayYear { get; set; }  // 2025

        // Earnings (BDT)
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Basic Salary (BDT)")]
        public decimal BasicSalaryBDT { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "House Rent (BDT)")]
        public decimal HouseRentBDT { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Medical Allowance (BDT)")]
        public decimal MedicalAllowanceBDT { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Transport Allowance (BDT)")]
        public decimal TransportAllowanceBDT { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Bonus (BDT)")]
        public decimal BonusBDT { get; set; } = 0;

        // Deductions (BDT)
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Deductions (BDT)")]
        public decimal DeductionsBDT { get; set; } = 0;

        // Status
        [Display(Name = "Is Paid")]
        public bool IsPaid { get; set; } = false;

        [Display(Name = "Generated Date")]
        public DateTime GeneratedDate { get; set; } = DateTime.Now;

        // Navigation Property
        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        // Computed Properties
        [NotMapped]
        [Display(Name = "Gross Salary (BDT)")]
        public decimal GrossSalaryBDT => BasicSalaryBDT + HouseRentBDT +
                                          MedicalAllowanceBDT + TransportAllowanceBDT +
                                          BonusBDT;

        [NotMapped]
        [Display(Name = "Net Salary (BDT)")]
        public decimal NetSalaryBDT => GrossSalaryBDT - DeductionsBDT;

        [NotMapped]
        public string PayPeriod => $"{PayMonth} {PayYear}";
    }
}
