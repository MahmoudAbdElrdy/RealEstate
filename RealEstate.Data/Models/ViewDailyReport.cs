using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class ViewDailyReport
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int SupervisorId { get; set; }
        public double? Value { get; set; }
        public string EmployeeSubmitted { get; set; }
        public DateTime? Date { get; set; }
        public string Notes { get; set; }
        public string SupervisorName { get; set; }
        public string EmployeeName { get; set; }
    }
}
