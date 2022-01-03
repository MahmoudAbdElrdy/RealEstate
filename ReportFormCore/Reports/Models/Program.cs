using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class Program
    {
        public Program()
        {
            ProgramDetails = new HashSet<ProgramDetail>();
        }

        public int Id { get; set; }
        public int ProjectUnitDescriptionId { get; set; }
        public string Name { get; set; }
        public double MeterCost { get; set; }
        public double TotalCost { get; set; }

        public virtual ProjectUnitDescription ProjectUnitDescription { get; set; }
        public virtual ICollection<ProgramDetail> ProgramDetails { get; set; }
    }
}
