using System;
using System.Collections.Generic;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class GoodsType
    {
        public GoodsType()
        {
            MaterialsPurchases = new HashSet<MaterialsPurchase>();
            ProjectExpenses = new HashSet<ProjectExpense>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMaterials { get; set; }

        public virtual ICollection<MaterialsPurchase> MaterialsPurchases { get; set; }
        public virtual ICollection<ProjectExpense> ProjectExpenses { get; set; }
    }
}
