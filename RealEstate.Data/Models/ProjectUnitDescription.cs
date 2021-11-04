using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class ProjectUnitDescription
    {
        public ProjectUnitDescription()
        {
            Programs = new HashSet<Program>();
            ProjectUnits = new HashSet<ProjectUnit>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public int Kitchen { get; set; }
        public int Bath { get; set; }
        public int Room { get; set; }
        public int FlatID { get; set; } 
        public bool? IsBooked { get; set; }  

        public virtual Project Project { get; set; }
        public virtual ICollection<Program> Programs { get; set; }
        public virtual ICollection<ProjectUnit> ProjectUnits { get; set; }
    }
}
