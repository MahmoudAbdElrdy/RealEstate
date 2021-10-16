using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class Employee
    {
        public Employee()
        {
            EmployeeSalaries = new HashSet<EmployeeSalary>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public DateTime WorkSince { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<EmployeeSalary> EmployeeSalaries { get; set; }
    }
}
