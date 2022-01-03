using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class EmployeeReward
    {
        public int Id { get; set; }
        public int EmployeeSalaryId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }

        public virtual EmployeeSalary EmployeeSalary { get; set; }
    }
}
