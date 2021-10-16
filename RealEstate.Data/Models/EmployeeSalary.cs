using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class EmployeeSalary
    {
        public EmployeeSalary()
        {
            EmployeePenalties = new HashSet<EmployeePenalty>();
            EmployeeRewards = new HashSet<EmployeeReward>();
        }

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<EmployeePenalty> EmployeePenalties { get; set; }
        public virtual ICollection<EmployeeReward> EmployeeRewards { get; set; }
    }
}
