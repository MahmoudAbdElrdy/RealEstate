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
        public double? Fixed { get; set; }
        public double? ProductionIncentive { get; set; }
        public double? Rewards { get; set; }
        public double? AdvancePayment { get; set; }
        public double? Sanctions { get; set; }
        public double? Delays { get; set; }
        public double? SocialInsurance { get; set; }
        public double? Holidays { get; set; }
        public double? Buffet { get; set; }
        public double? Commission { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<EmployeePenalty> EmployeePenalties { get; set; }
        public virtual ICollection<EmployeeReward> EmployeeRewards { get; set; }
    }
}
