using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class DailyReport
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int SupervisorId { get; set; }
        public double? Value { get; set; }
        public string EmployeeSubmitted { get; set; }
        public DateTime? Date { get; set; }
        public string Notes { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Supervisor Supervisor { get; set; }
    }
}
