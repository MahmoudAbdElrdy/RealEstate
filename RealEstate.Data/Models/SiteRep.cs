using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class SiteRep
    {
        public SiteRep()
        {
            ProjectExpenses = new HashSet<ProjectExpense>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<ProjectExpense> ProjectExpenses { get; set; }
    }
}
