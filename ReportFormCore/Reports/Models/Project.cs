using System;
using System.Collections.Generic;



namespace ReportFormCore.Data.Models
{
    public partial class Project
    {
        public Project()
        {
            MaterialsAllocations = new HashSet<MaterialsAllocation>();
            ProjectExpenses = new HashSet<ProjectExpense>();
            ProjectUnitDescriptions = new HashSet<ProjectUnitDescription>();
            ProjectVisits = new HashSet<ProjectVisit>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Floors { get; set; }
        public int? ApartmentNumber { get; set; }

        public virtual ICollection<MaterialsAllocation> MaterialsAllocations { get; set; }
        public virtual ICollection<ProjectExpense> ProjectExpenses { get; set; }
        public virtual ICollection<ProjectUnitDescription> ProjectUnitDescriptions { get; set; }
        public virtual ICollection<ProjectVisit> ProjectVisits { get; set; }
    }
}
