using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class EmployeePenalty
    {
        public int Id { get; set; }
        public int EmployeeSalaryId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }

        public virtual EmployeeSalary EmployeeSalary { get; set; }
    }
}
