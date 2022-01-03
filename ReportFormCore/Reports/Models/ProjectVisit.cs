using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class ProjectVisit
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public bool Visited { get; set; }
        public string Notes { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Project Project { get; set; }
    }
}
