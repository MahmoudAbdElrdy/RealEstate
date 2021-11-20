using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class Supervisor
    {
        public Supervisor()
        {
            DailyReports = new HashSet<DailyReport>();
            SupervisorDetails = new HashSet<SupervisorDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string NationalNumber { get; set; }
        public string Job { get; set; }

        public virtual ICollection<DailyReport> DailyReports { get; set; }
        public virtual ICollection<SupervisorDetail> SupervisorDetails { get; set; }
    }
}
