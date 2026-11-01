namespace NexusWork.Models
{
    public static class DropdownData
    {
        // Bangladesh Banks
        public static List<string> BankNames => new List<string>
        {
            "Dutch-Bangla Bank Limited",
            "BRAC Bank Limited",
            "Eastern Bank Limited",
            "Islami Bank Bangladesh Limited",
            "City Bank Limited",
            "Prime Bank Limited",
            "Pubali Bank Limited",
            "Sonali Bank Limited",
            "Janata Bank Limited",
            "Agrani Bank Limited",
            "Rupali Bank Limited",
            "United Commercial Bank Limited",
            "Mutual Trust Bank Limited",
            "Bank Asia Limited",
            "Standard Chartered Bangladesh"
        };

        // Departments
        public static List<string> Departments => new List<string>
        {
            "Human Resources",
            "Finance",
            "Information Technology",
            "Marketing",
            "Sales",
            "Operations",
            "Administration",
            "Research & Development",
            "Ungabunga"
        };

        // Designations
        public static List<string> Designations => new List<string>
        {
            "Junior Executive",
            "Executive",
            "Senior Executive",
            "Assistant Manager",
            "Manager",
            "Senior Manager",
            "Assistant General Manager",
            "General Manager",
            "Director"
        };

        // Genders
        public static List<string> Genders => new List<string>
        {
            "Male",
            "Female",
            "Other"
        };

        // Task Priorities
        public static List<string> TaskPriorities => new List<string>
        {
            "Low",
            "Medium",
            "High"
        };

        // Task Statuses
        public static List<string> TaskStatuses => new List<string>
        {
            "Pending",
            "In Progress",
            "Completed"
        };

        // Pay Months
        public static List<string> PayMonths => new List<string>
        {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"
        };

        // Document Categories
        public static List<string> DocumentCategories => new List<string>
        {
            "Policy",
            "Handbook",
            "Template",
            "Form"
        };
    }
}
