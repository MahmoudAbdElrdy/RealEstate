using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class ProjectUnit
    {
        public ProjectUnit()
        {
            Contracts = new HashSet<Contract>();
        }

        public int Id { get; set; }
        public int ProjectUnitDescriptionId { get; set; }
        public int Number { get; set; }
        public int FloorNumber { get; set; }

        public virtual ProjectUnitDescription ProjectUnitDescription { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
    }
}
