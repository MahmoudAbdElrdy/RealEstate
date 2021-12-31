using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class SupervisorDetail
    {
        public int Id { get; set; }
        public int SupervisorId { get; set; }
        public double? Credit { get; set; }
        public double? Debt { get; set; }
        public double? Net { get; set; }
        public DateTime? Date { get; set; }

        public virtual Supervisor Supervisor { get; set; }
    }
}
