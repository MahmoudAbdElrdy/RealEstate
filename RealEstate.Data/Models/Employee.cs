using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class Employee
    {
        public Employee()
        {
            DailyReports = new HashSet<DailyReport>();
            EmployeeSalaries = new HashSet<EmployeeSalary>();
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime WorkSince { get; set; }
        public string Phone { get; set; }
        public string PassWord { get; set; }
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<DailyReport> DailyReports { get; set; }
        public virtual ICollection<EmployeeSalary> EmployeeSalaries { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
