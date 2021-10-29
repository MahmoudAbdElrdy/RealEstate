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
        public int? Fixed { get; set; }
        public int? ProductionIncentive { get; set; }
        public int? Rewards { get; set; }
        public int? AdvancePayment { get; set; }
        public int? Sanctions { get; set; }
        public int? Delays { get; set; }
        public int? SocialInsurance { get; set; }
        public int? Holidays { get; set; }
        public int? Buffet { get; set; }
        public int? Commission { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<EmployeePenalty> EmployeePenalties { get; set; }
        public virtual ICollection<EmployeeReward> EmployeeRewards { get; set; }
    }
}
